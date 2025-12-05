using BillByte.Data;
using BillByte.DTO;
using BillByte.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;


namespace BillByte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableOrdersController : ControllerBase
    {
        //private readonly ApplicationDbContext _db;
        //private readonly IHubContext<YourNamespace.Hubs.TableHub> _hub;

        //public TableOrdersController(ApplicationDbContext db, IHubContext<YourNamespace.Hubs.TableHub> hub)
        //{
        //    _db = db;
        //    _hub = hub;
        //}

        //// GET api/tableorders/{tableNumber}
        //[HttpGet("{tableNumber}")]
        //public async Task<IActionResult> GetByTable(string tableNumber)
        //{
        //    var order = await _db.TableOrders
        //                 .Include(o => o.Items)
        //                 .Where(o => o.TableNumber == tableNumber && (o.Status == "Occupied" || o.Status == "Available"))
        //                 .OrderByDescending(o => o.Id)
        //                 .FirstOrDefaultAsync();

        //    if (order == null) return Ok(null);

        //    return Ok(new
        //    {
        //        order.TableNumber,
        //        order.OrderType,
        //        startTime = order.StartTime,
        //        subtotal = order.Subtotal,
        //        tax = order.Tax,
        //        total = order.Total,
        //        status = order.Status,
        //        items = order.Items.Select(i => new
        //        {
        //            itemId = i.ItemId,
        //            itemName = i.ItemName,
        //            itemCost = i.ItemCost,
        //            qty = i.Qty,
        //            imageUrl = i.ImageUrl
        //        })
        //    });
        //}

        //// POST api/tableorders/save
        //[HttpPost("save")]
        //public async Task<IActionResult> Save([FromBody] TableOrderDto dto)
        //{
        //    // find existing open order for table
        //    var existing = await _db.TableOrders
        //                       .Include(o => o.Items)
        //                       .Where(o => o.TableNumber == dto.TableNumber && (o.Status == "Occupied" || o.Status == "Available"))
        //                       .OrderByDescending(o => o.Id)
        //                       .FirstOrDefaultAsync();

        //    if (existing == null)
        //    {
        //        existing = new TableOrder
        //        {
        //            TableNumber = dto.TableNumber,
        //            OrderType = dto.OrderType,
        //            StartTime = dto.StartTime,
        //            Subtotal = dto.Subtotal,
        //            Tax = dto.Tax,
        //            Total = dto.Total,
        //            Status = dto.Status ?? "Occupied"
        //        };
        //        _db.TableOrders.Add(existing);
        //        await _db.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        // update totals and startTime & orderType & status
        //        existing.OrderType = dto.OrderType;
        //        existing.StartTime = dto.StartTime;
        //        existing.Subtotal = dto.Subtotal;
        //        existing.Tax = dto.Tax;
        //        existing.Total = dto.Total;
        //        existing.Status = dto.Status ?? existing.Status;
        //        // remove existing items and add new
        //        _db.OrderItems.RemoveRange(existing.Items);
        //        existing.Items.Clear();
        //        await _db.SaveChangesAsync();
        //    }

        //    // add new items
        //    foreach (var it in dto.Items ?? new List<OrderItemDto>())
        //    {
        //        var oi = new OrderItem
        //        {
        //            TableOrderId = existing.Id,
        //            ItemId = it.ItemId,
        //            ItemName = it.ItemName,
        //            ItemCost = it.ItemCost,
        //            Qty = it.Qty,
        //            ImageUrl = it.ImageUrl
        //        };
        //        _db.OrderItems.Add(oi);
        //    }
        //    await _db.SaveChangesAsync();

        //    // notify other clients that table order updated (optional)
        //    await _hub.Clients.All.SendAsync("ReceiveTableUpdate", new { table = dto.TableNumber, status = existing.Status });

        //    return Ok(new { success = true });
        //}

    }
}
