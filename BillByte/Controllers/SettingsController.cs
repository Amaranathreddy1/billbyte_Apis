using BillByte.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BillByte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IBusinessUnitSettingRepository _repo;

        public SettingsController(IBusinessUnitSettingRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Settings/dashboard/1
        [HttpGet("dashboard/{userId:int}")]
        public async Task<IActionResult> GetDashboardSettings(int userId)
        {
            var settings = await _repo.GetDashboardSettingsForUserAsync(userId);
            return Ok(settings);
        }
    }
}
