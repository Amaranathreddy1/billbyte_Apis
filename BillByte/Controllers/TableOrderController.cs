using BillByte.DTO;
using BillByte.Hubs;
using BillByte.Interface;
using BillByte.Model;
using BillByte.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BillByte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableOrderController : ControllerBase
    {
        private readonly ITableOrderRepository _repo;

        public TableOrderController(ITableOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{tableNumber}")]
        public async Task<IActionResult> GetOrder(string tableNumber)
        {
            var data = await _repo.GetOrderAsync(tableNumber);
            return Ok(data);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveOrder([FromBody] SaveOrderRequest dto)
        {
            var id = await _repo.SaveOrderAsync(dto);
            return Ok(new { success = true, orderId = id });
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartOrder([FromBody] CreateTableOrderDto req)
        {
            if (req == null)
                return BadRequest("REQ IS NULL");

            int orderId = await _repo.CreateOrderAsync(req);

            return Ok(new { orderId });
        }

        // Expose get latest order for a table (if needed)
        [HttpGet("table/{tableNumber}")]
        public async Task<IActionResult> GetOrderByTable(string tableNumber)
        {
            var items = await _repo.GetOrderByTableAsync(tableNumber);
            return Ok(items);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveOrders()
        {
            var result = await _repo.GetActiveOrdersAsync();
            return Ok(result);
        }

        [HttpPost("startOrUpdate")]
        public async Task<IActionResult> StartOrUpdateOrder([FromBody] CreateTableOrderDto req)
        {
            var id = await _repo.startOrUpdate(req);
            return Ok(new { orderId = id });
        }


    }

}

