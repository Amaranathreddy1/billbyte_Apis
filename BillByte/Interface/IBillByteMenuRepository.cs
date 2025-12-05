using BillByte.Model;

namespace BillByte.Interface
{
    public interface IBillByteMenuRepository
    {
        Task<List<BillByteMenu>> GetAllAsync();
        Task<BillByteMenu> GetByIdAsync(int id);
        Task<BillByteMenu> AddAsync(BillByteMenu item);
        Task<BillByteMenu> UpdateAsync(BillByteMenu item);
        Task DeleteAsync(int id);
        Task<List<BillByteMenu>> GetByFoodTypeAsync(int typeId);
    }
}
