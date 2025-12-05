using BillByte.DTO;
using BillByte.Interface;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;


namespace BillByte.Repository
{
    public class TableOrderRepository : ITableOrderRepository
    {
        private readonly string _conn;

        public TableOrderRepository(IConfiguration cfg)
        {
            _conn = cfg.GetConnectionString("DBConn");
        }

        public async Task<int> CreateAsync(CreateTableOrderDto dto)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"
            INSERT INTO TableOrders
            (TableNumber, ZoneType, UserId, ItemIds, TotalCost,
             StartTime, EndTime, SpentTime, PaymentMode, UserType, Status)
            VALUES
            (@TableNumber, @ZoneType, @UserId, @ItemIds, @TotalCost,
             @StartTime, @EndTime, @SpentTime, @PaymentMode, @UserType, @Status);
            SELECT SCOPE_IDENTITY();
        ", con);

            var endTime = DateTime.UtcNow;
            var spentSeconds = (int)(endTime - dto.StartTime).TotalSeconds;

            cmd.Parameters.AddWithValue("@TableNumber", dto.TableNumber);
            cmd.Parameters.AddWithValue("@ZoneType", dto.ZoneType);
            cmd.Parameters.AddWithValue("@UserId", dto.UserId);
            cmd.Parameters.AddWithValue("@ItemIds", (object?)dto.ItemIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalCost", dto.TotalCost);
            cmd.Parameters.AddWithValue("@StartTime", dto.StartTime);
            cmd.Parameters.AddWithValue("@EndTime", endTime);
            cmd.Parameters.AddWithValue("@SpentTime", spentSeconds);
            cmd.Parameters.AddWithValue("@PaymentMode", (object?)dto.PaymentMode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserType", dto.UserType);
            cmd.Parameters.AddWithValue("@Status", "Billed");

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<dynamic>> GetTableStatusAsync(int userId)
        {
            var list = new List<dynamic>();

            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"
            ;WITH lastRow AS (
              SELECT *, ROW_NUMBER() OVER (PARTITION BY TableNumber ORDER BY LastUpdated DESC) AS rn
              FROM TableOrders
              WHERE UserId = @UserId
            )
            SELECT TableNumber, ZoneType, Status, StartTime, EndTime, SpentTime, TotalCost
            FROM lastRow WHERE rn = 1;
        ", con);

            cmd.Parameters.AddWithValue("@UserId", userId);
            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new
                {
                    TableNumber = dr["TableNumber"].ToString(),
                    ZoneType = dr["ZoneType"].ToString(),
                    Status = dr["Status"].ToString(),
                    StartTime = dr["StartTime"] as DateTime?,
                    EndTime = dr["EndTime"] as DateTime?,
                    SpentTime = dr["SpentTime"] as int?,
                    TotalCost = dr["TotalCost"] as decimal?
                });
            }

            return list;
        }
        public async Task UpdateStatusAsync(string tableName, string status)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                UPDATE TableOrders
                SET Status = @Status,
                    LastUpdated = SYSUTCDATETIME()
                WHERE TableNumber = @TableNumber;
            ", con))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@TableNumber", tableName);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<TableOrderItemDto>> GetOrderByTableAsync(string tableNumber)
        {
            var result = new List<TableOrderItemDto>();

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetOrderByTable", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableNumber", tableNumber);

                await con.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        result.Add(new TableOrderItemDto
                        {
                            TableOrderId = Convert.ToInt32(dr["TableOrderId"]),
                            TableNumber = dr["TableNumber"].ToString(),
                            ItemId = Convert.ToInt32(dr["ItemId"]),
                            ItemName = dr["ItemName"].ToString(),
                            ItemCost = Convert.ToDecimal(dr["ItemCost"]),
                            ImageUrl = dr["ImageUrl"].ToString(),
                            Qty = Convert.ToInt32(dr["Qty"])
                        });
                    }
                }
            }

            return result;
        }

        public async Task<int> CreateOrderAsync(CreateTableOrderDto req)
        {
            int orderId;

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_CreateOrder", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TableNumber", req.TableNumber);
                cmd.Parameters.AddWithValue("@ZoneType", req.ZoneType);
                cmd.Parameters.AddWithValue("@UserId", req.UserId);
                cmd.Parameters.AddWithValue("@SubTotal", req.SubTotal);
                cmd.Parameters.AddWithValue("@Tax", req.Tax);
                cmd.Parameters.AddWithValue("@TotalCost", req.TotalCost);
                cmd.Parameters.AddWithValue("@StartTime", req.StartTime);
                cmd.Parameters.AddWithValue("@PaymentMode", req.PaymentMode); 
                cmd.Parameters.AddWithValue("@UserType", req.UserType);
                cmd.Parameters.AddWithValue("@Status", req.Status);

                await con.OpenAsync();
                orderId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            foreach (var item in req.Items)
            {
                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand("sp_AddOrderItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                    cmd.Parameters.AddWithValue("@Qty", item.Qty);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return orderId;
        }
        public async Task<TableOrderDto?> GetOrderAsync(string tableNumber)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("sp_GetOrderByTable", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TableNumber", tableNumber);

            await con.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();

            TableOrderDto order = null;

            // ---- Read Order Header ----
            if (await dr.ReadAsync())
            {
                order = new TableOrderDto
                {
                    TableOrderId = Convert.ToInt32(dr["TableOrderId"]),
                    TableNumber = dr["TableNumber"].ToString(),
                    ZoneType = dr["ZoneType"].ToString(),
                    UserId = Convert.ToInt32(dr["UserId"]),
                    ItemIds = dr["ItemIds"].ToString(),
                    TotalCost = Convert.ToDecimal(dr["TotalCost"]),
                    StartTime = dr["StartTime"] as DateTime?,
                    UserType = dr["UserType"].ToString(),
                    PaymentMode = dr["PaymentMode"].ToString(),
                    Status = dr["Status"].ToString()
                };
            }

            // Move to second result (Items)
            if (await dr.NextResultAsync())
            {
                while (await dr.ReadAsync())
                {
                    order.Items.Add(new TableOrderItemDto
                    {
                        TableOrderId = Convert.ToInt32(dr["TableOrderId"]),
                        TableNumber = dr["TableNumber"].ToString(),
                        ItemId = Convert.ToInt32(dr["ItemId"]),
                        ItemName = dr["ItemName"].ToString(),
                        ItemCost = Convert.ToDecimal(dr["ItemCost"]),
                        ImageUrl = dr["ImageUrl"].ToString(),
                        Qty = Convert.ToInt32(dr["Qty"])
                    });
                }
            }

            return order;
        }

        // SAVE ORDER
        public async Task<int> SaveOrderAsync(SaveOrderRequest dto)
        {
            int orderId;

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_CreateOrUpdateOrder", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TableNumber", dto.TableNumber);
                cmd.Parameters.AddWithValue("@ZoneType", dto.ZoneType);
                cmd.Parameters.AddWithValue("@UserId", dto.UserId);
                cmd.Parameters.AddWithValue("@ItemIds", dto.ItemIds ?? "");
                cmd.Parameters.AddWithValue("@TotalCost", dto.TotalCost);
                cmd.Parameters.AddWithValue("@StartTime", dto.StartTime);
                cmd.Parameters.AddWithValue("@UserType", dto.UserType);
                cmd.Parameters.AddWithValue("@PaymentMode", dto.PaymentMode);
                cmd.Parameters.AddWithValue("@Status", dto.Status);

                await con.OpenAsync();
                orderId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            // SAVE ORDER ITEMS
            foreach (var item in dto.Items)
            {
                using var con = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_AddOrderItem", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                cmd.Parameters.AddWithValue("@Qty", item.Qty);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return orderId;
        }

        public async Task<List<ActiveOrderDto>> GetActiveOrdersAsync()
        {
            var list = new List<ActiveOrderDto>();

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetActiveOrders", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        var dto = new ActiveOrderDto
                        {
                            TableOrderId = Convert.ToInt32(dr["TableOrderId"]),
                            TableNumber = dr["TableNumber"].ToString(),
                            ZoneType = dr["ZoneType"].ToString(),
                            SubTotal = Convert.ToDecimal(dr["SubTotal"]),
                            Tax = Convert.ToDecimal(dr["Tax"]),
                            TotalCost = Convert.ToDecimal(dr["TotalCost"]),
                            StartTime = Convert.ToDateTime(dr["StartTime"]),
                            UserType = dr["UserType"].ToString(),
                            Items = JsonSerializer.Deserialize<List<TableOrderItemDto>>(dr["Items"].ToString())
                        };

                        list.Add(dto);
                    }
                }
            }

            return list;
        }
        public async Task<int?> GetActiveOrderId(string tableNumber)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"SELECT TOP 1 TableOrderId 
                                     FROM TableOrders 
                                     WHERE TableNumber=@t AND UserType='OrderIn'
                                     ORDER BY TableOrderId DESC", con);

            cmd.Parameters.AddWithValue("@t", tableNumber);
            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return result == null ? null : Convert.ToInt32(result);
        }

        public async Task<int> startOrUpdate(CreateTableOrderDto dto)
        {
            int? existingOrderId = await GetActiveOrderId(dto.TableNumber);

            if (existingOrderId != null)
            {
                // UPDATE existing order
                foreach (var item in dto.Items)
                {
                    using var con = new SqlConnection(_conn);
                    using var cmd = new SqlCommand("sp_UpdateOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", existingOrderId.Value);
                    cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                    cmd.Parameters.AddWithValue("@Qty", item.Qty);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

                return existingOrderId.Value;
            }
            else
            {
                // CREATE new order
                return await CreateOrderAsync(dto);
            }
        }


    }

}

