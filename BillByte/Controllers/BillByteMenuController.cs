using BillByte.Interface;
using BillByte.Model;
using Microsoft.AspNetCore.Mvc;

namespace BillByte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillByteMenuController : ControllerBase
    {
        private readonly IBillByteMenuRepository _repo;
        private readonly IWebHostEnvironment _env;

        public BillByteMenuController(IBillByteMenuRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _repo.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BillByteMenu model)
        {
            model.CreatedDate = DateTime.UtcNow;
            var created = await _repo.AddAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.ItemId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BillByteMenu model)
        {
            model.ItemId = id;
            var updated = await _repo.UpdateAsync(model);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        // Image upload endpoint
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest();
            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create)) await file.CopyToAsync(stream);
            var url = $"/uploads/{fileName}";
            return Ok(new { url });
        }

        [HttpGet("type/{foodTypeId}")]
        public async Task<IActionResult> GetByFoodType(int foodTypeId)
        {
            var items = await _repo.GetByFoodTypeAsync(foodTypeId);
            return Ok(items);
        }
    }

}
