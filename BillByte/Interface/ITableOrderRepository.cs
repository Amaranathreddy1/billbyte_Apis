using BillByte.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BillByte.Interface
{
    public interface ITableOrderRepository
    {
        Task<List<TableOrderItemDto>> GetOrderByTableAsync(string tableNumber);
        Task<int> CreateOrderAsync(CreateTableOrderDto items);
        Task<TableOrderDto?> GetOrderAsync(string tableNumber);
        Task<int> SaveOrderAsync(SaveOrderRequest dto);
        Task<List<ActiveOrderDto>> GetActiveOrdersAsync();
        Task<int> startOrUpdate(CreateTableOrderDto dto);
    }
}
