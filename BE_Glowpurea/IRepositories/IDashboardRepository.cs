using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IDashboardRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<int> GetTotalUsersAsync();
        Task<int> GetNewUsersThisMonthAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetActiveProductsAsync();
        Task<int> GetOutOfStockProductsAsync();
    }
}
