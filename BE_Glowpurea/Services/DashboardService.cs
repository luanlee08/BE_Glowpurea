using BE_Glowpurea.Dtos.Dashboard;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;

namespace BE_Glowpurea.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardOverviewResponse> GetOverviewAsync()
        {
            var orders = await _repo.GetOrdersAsync();

            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfToday = now.Date;

            return new DashboardOverviewResponse
            {
                Orders = new OrderStatsDto
                {
                    TotalOrders = orders.Count,
                    Pending = orders.Count(o => o.StatusId == 1),
                    Confirmed = orders.Count(o => o.StatusId == 2),
                    Shipped = orders.Count(o => o.StatusId == 3),
                    Delivered = orders.Count(o => o.StatusId == 4),
                    Cancelled = orders.Count(o => o.StatusId == 5)
                },

                Revenue = new RevenueStatsDto
                {
                    TotalRevenue = orders
                        .Where(o => o.StatusId == 4)
                        .Sum(o => o.TotalAmount),

                    ThisMonthRevenue = orders
                        .Where(o => o.StatusId == 4 && o.CreatedAt >= startOfMonth)
                        .Sum(o => o.TotalAmount),

                    TodayRevenue = orders
                        .Where(o => o.StatusId == 4 && o.CreatedAt >= startOfToday)
                        .Sum(o => o.TotalAmount),

                    MonthlyRevenue = Enumerable.Range(1, 12)
                        .Select(m =>
                            orders
                                .Where(o =>
                                    o.StatusId == 4 &&
                                    o.CreatedAt.Year == now.Year &&
                                    o.CreatedAt.Month == m)
                                .Sum(o => o.TotalAmount)
                        )
                        .ToList()
                },

                Users = new UserStatsDto
                {
                    TotalUsers = await _repo.GetTotalUsersAsync(),
                    NewUsersThisMonth = await _repo.GetNewUsersThisMonthAsync()
                },

                Products = new ProductStatsDto
                {
                    TotalProducts = await _repo.GetTotalProductsAsync(),
                    ActiveProducts = await _repo.GetActiveProductsAsync(),
                    OutOfStock = await _repo.GetOutOfStockProductsAsync()
                }
            };
        }
    }
}