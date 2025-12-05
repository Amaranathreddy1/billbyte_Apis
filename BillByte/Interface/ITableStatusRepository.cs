using BillByte.DTO;

namespace BillByte.Interface
{
    public interface ITableStatusRepository
    {
        Task UpdateStatusAsync(string tableName, string status);
        Task<string> GetTableStatusAsync(string tableName);
    }
}
