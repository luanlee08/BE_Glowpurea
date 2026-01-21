using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly DbGlowpureaContext _context;

        public OrderRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllForAdminAsync()
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.Status)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdForAdminAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) 
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Order?> GetByIdWithItemsAsync(int orderId, int accountId)
        {
            return await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o =>
                    o.OrderId == orderId &&
                    o.AccountId == accountId
                );
        }

        public async Task<(List<Order> Orders, int Total)> GetPagedForAdminAsync(
    int page,
    int pageSize
)
        {
            var query = _context.Orders
                .Include(o => o.Account)
                .Include(o => o.Status)
                .OrderByDescending(o => o.CreatedAt);

            var total = await query.CountAsync();

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, total);
        }

    }
}
