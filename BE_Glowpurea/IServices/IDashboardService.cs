using BE_Glowpurea.Dtos.Dashboard;

namespace BE_Glowpurea.IServices
{
    public interface IDashboardService
    {
        Task<DashboardOverviewResponse> GetOverviewAsync();
    }
}
