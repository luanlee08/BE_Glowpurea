using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DbGlowpureaContext _context;

        public DashboardRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Accounts
                .CountAsync(a =>
                    !a.IsDeleted &&
                    a.RoleId == 2
                );
        }

        public async Task<int> GetNewUsersThisMonthAsync()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            return await _context.Accounts
                .CountAsync(a => !a.IsDeleted && a.CreatedAt >= startOfMonth);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products
                .CountAsync(p => !p.IsDeleted);
        }

        public async Task<int> GetActiveProductsAsync()
        {
            return await _context.Products
                .CountAsync(p => !p.IsDeleted && p.ProductStatus == "Active");
        }

        public async Task<int> GetOutOfStockProductsAsync()
        {
            return await _context.Products
                .CountAsync(p => !p.IsDeleted && p.Quantity == 0);
        }

        public async Task<List<Account>> GetUsersAsync()
        {
            return await _context.Accounts
                .Where(a => !a.IsDeleted && a.RoleId == 2)
                .ToListAsync();
        }

    }
}
