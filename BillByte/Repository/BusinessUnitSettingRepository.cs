using BillByte.Interface;
using BillByte.Model;
using System.Data;
using System.Data.SqlClient;

namespace BillByte.Repository
{
    public class BusinessUnitSettingRepository : IBusinessUnitSettingRepository
    {

        private readonly string _connectionString;

        public BusinessUnitSettingRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConn");
        }

        public async Task<DashboardBuSettings> GetDashboardSettingsForUserAsync(int userId)
        {
            var result = new DashboardBuSettings();

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetDashboardSettingsForUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                await con.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        result.IsTableServeNeeded = Convert.ToBoolean(dr["IsTableServeNeeded"]);
                        result.NonAcTables = Convert.ToInt32(dr["NonAcTables"]);
                        result.AcTables = Convert.ToInt32(dr["AcTables"]);
                    }
                }
            }

            return result;
        }
    }
}

