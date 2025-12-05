using BillByte.Interface;
using BillByte.Repository;
using Microsoft.AspNetCore.Mvc;

//https://www.behance.net/gallery/184648067/Cloud-Kitchen-POS-System-Design

namespace BillByte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly BillByteMenuRepository _repo;
        public AnalyticsController(BillByteMenuRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("sales/last7days")]
        public async Task<IActionResult> GetSalesLast7Days()
        {
            try
            {
                var data = await _repo.GetSalesByTypeLast7DaysAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"SERVER ERROR: {ex.Message}");
            }
        }
    }

}
