using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IOrderRepository
    {
        // ===== QUERY =====
        Task<List<Order>> GetAllForAdminAsync();
        Task<Order?> GetByIdForAdminAsync(int orderId);
        Task<Order?> GetByIdAsync(int orderId);
        Task<Order?> GetByIdWithItemsAsync(int orderId, int accountId);
        // ===== COMMAND =====
        Task AddAsync(Order order);
        Task SaveChangesAsync();
        Task<(List<Order> Orders, int Total)> GetPagedForAdminAsync(int page, int pageSize);

    }
}
