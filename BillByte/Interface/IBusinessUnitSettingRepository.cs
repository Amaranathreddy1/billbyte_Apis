using BillByte.Model;

namespace BillByte.Interface
{
    public interface IBusinessUnitSettingRepository
    {
        Task<DashboardBuSettings> GetDashboardSettingsForUserAsync(int userId);
    }
}
