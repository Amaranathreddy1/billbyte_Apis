using BillByte.DTO;
using BillByte.Interface;
using BillByte.Model;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BillByte.Repository
{
    public class BillByteMenuRepository : IBillByteMenuRepository
    {
        private readonly string _connectionString;

        public BillByteMenuRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConn");
        }

        // ✔ GET ALL MENU ITEMS
        public async Task<List<BillByteMenu>> GetAllAsync()
        {
            var list = new List<BillByteMenu>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetBillByteMenu", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new BillByteMenu
                        {
                            ItemId = Convert.ToInt32(dr["ItemId"]),
                            ItemName = dr["ItemName"].ToString(),
                            ItemTypeId = Convert.ToInt32(dr["ItemTypeId"]),
                            ItemCost = Convert.ToDecimal(dr["ItemCost"]),
                            GSTPercentage = dr["GSTPercentage"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["GSTPercentage"]),
                            CGSTPercentage = dr["CGSTPercentage"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["CGSTPercentage"]),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                            ImageUrl = dr["ImageUrl"]?.ToString(),
                            CreatedBy = dr["CreatedBy"]?.ToString()//,
                            //FoodTypeName = dr["FoodTypeName"]?.ToString()
                        });
                    }
                }
            }
            return list;
        }

        // ✔ GET BY ID
        public async Task<BillByteMenu> GetByIdAsync(int id)
        {
            BillByteMenu item = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetBillByteMenuById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", id);

                await con.OpenAsync();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        item = new BillByteMenu
                        {
                            ItemId = Convert.ToInt32(dr["ItemId"]),
                            ItemName = dr["ItemName"].ToString(),
                            ItemTypeId = Convert.ToInt32(dr["ItemTypeId"]),
                            ItemCost = Convert.ToDecimal(dr["ItemCost"]),
                            GSTPercentage = dr["GSTPercentage"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["GSTPercentage"]),
                            CGSTPercentage = dr["CGSTPercentage"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["CGSTPercentage"]),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                            ImageUrl = dr["ImageUrl"]?.ToString(),
                            CreatedBy = dr["CreatedBy"]?.ToString()
                        };
                    }
                }
            }
            return item;
        }

        // ✔ INSERT
        public async Task<BillByteMenu> AddAsync(BillByteMenu item)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertBillByteMenu", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemName", item.ItemName ?? "");
                cmd.Parameters.AddWithValue("@ItemTypeId", item.ItemTypeId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ItemCost", item.ItemCost);
                cmd.Parameters.AddWithValue("@GSTPercentage", item.GSTPercentage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CGSTPercentage", item.CGSTPercentage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", item.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy ?? "Admin");
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                item.ItemId = Convert.ToInt32(result);
            }

            return item;
        }

        // ✔ UPDATE
        public async Task<BillByteMenu> UpdateAsync(BillByteMenu item)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateBillByteMenu", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                cmd.Parameters.AddWithValue("@ItemTypeId", item.ItemTypeId);
                cmd.Parameters.AddWithValue("@ItemCost", item.ItemCost);
                cmd.Parameters.AddWithValue("@GSTPercentage", item.GSTPercentage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CGSTPercentage", item.CGSTPercentage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageUrl", item.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return item;
        }

        // ✔ DELETE
        public async Task DeleteAsync(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteBillByteMenu", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", id);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<BillByteMenu>> GetByFoodTypeAsync(int typeId)
        {
            var result = new List<BillByteMenu>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ItemId, ItemName, ItemTypeId, ItemCost, GSTPercentage, CGSTPercentage,
                       CreatedDate, CreatedBy, ImageUrl
                FROM BillByteMenu
                WHERE ItemTypeId = @typeId
                ORDER BY ItemName", con))
            {
                cmd.Parameters.AddWithValue("@typeId", typeId);

                con.Open();
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        result.Add(new BillByteMenu
                        {
                            ItemId = Convert.ToInt32(dr["ItemId"]),
                            ItemName = dr["ItemName"].ToString(),
                            ItemTypeId = Convert.ToInt32(dr["ItemTypeId"]),
                            ItemCost = Convert.ToDecimal(dr["ItemCost"]),
                            GSTPercentage = Convert.ToDecimal(dr["GSTPercentage"]),
                            CGSTPercentage = Convert.ToDecimal(dr["CGSTPercentage"]),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            ImageUrl = dr["ImageUrl"].ToString()
                        });
                    }
                }
            }

            return result;
        }

        // In repository (assume IConfiguration _config)
        public async Task<List<SalesPointDto>> GetSalesByTypeLast7DaysAsync()
        {
            var list = new List<SalesPointDto>();
            using (SqlConnection con = new SqlConnection(_connectionString))

            //using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand("sp_GetSalesByTypeLast7Days", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new SalesPointDto
                        {
                            DayDate = Convert.ToDateTime(dr["DayDate"]),
                            DayName = dr["DayName"].ToString(),
                            OrderInCount = Convert.ToInt32(dr["OrderInCount"]),
                            DeliveryCount = Convert.ToInt32(dr["DeliveryCount"]),
                            ParcelCount = Convert.ToInt32(dr["ParcelCount"])
                        });
                    }
                }
            }
            return list;
        }


    }
}
