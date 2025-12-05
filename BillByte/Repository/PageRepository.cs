
using BillByte.Model;
using System.Data;
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SqlConnection = System.Data.SqlClient.SqlConnection;


namespace BillByte.Repository
{
    public class PageRepository
    {
        private readonly IConfiguration _config;

        public PageRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Page>> GetPagesAsync()
        {
            var result = new List<Page>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBConn")))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("GetPages", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (System.Data.SqlClient.SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            result.Add(new Page
                            {
                                PageId = Convert.ToInt32(dr["PageId"]),
                                PageName = dr["PageName"].ToString()
                            });
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<FoodType>> GetFoodTypesAsync()
        {
            var result = new List<FoodType>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBConn")))
            using (SqlCommand cmd = new SqlCommand("SELECT FoodTypeId, FoodTypeName FROM FoodTypes ORDER BY DisplayOrder", con))
            {
                con.Open(); 
                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        result.Add(new FoodType
                        {
                            FoodTypeId = Convert.ToInt32(dr["FoodTypeId"]),
                            FoodTypeName = dr["FoodTypeName"].ToString()
                        });
                    }
                }
            }

            return result;
        }

    }

}
