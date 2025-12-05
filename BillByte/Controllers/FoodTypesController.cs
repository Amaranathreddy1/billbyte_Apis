using BillByte.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BillByte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypesController : ControllerBase
    {
        private readonly FoodTypeRepository _repo;

        public FoodTypesController(FoodTypeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodTypes()
        {
            var foodTypes = await _repo.GetFoodTypesAsync();
            return Ok(foodTypes);
        }
    }
}
