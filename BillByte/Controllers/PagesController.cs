using BillByte.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BillByte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly PageRepository _repo;

        public PagesController(PageRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetPages()
        {
            var pages = await _repo.GetPagesAsync();
            return Ok(pages);
        }
    }

}
