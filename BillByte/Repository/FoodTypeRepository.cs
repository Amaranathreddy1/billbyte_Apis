using BillByte.Model;
using Microsoft.Data.SqlClient;
using System.Data;


namespace BillByte.Repository
{
    public class FoodTypeRepository
    {
        private readonly IConfiguration _config;

        public FoodTypeRepository(IConfiguration config)
        {
            _config = config;
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
