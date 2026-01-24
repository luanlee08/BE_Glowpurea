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
            // ================== LOAD DATA ==================
            var orders = await _repo.GetOrdersAsync();
            var users = await _repo.GetUsersAsync(); // chỉ customer

            var now = DateTime.Now;

            // ================== TIME ==================
            var startOfToday = now.Date;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            // Tuần bắt đầu từ Thứ 2
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + 1);
            if (now.DayOfWeek == DayOfWeek.Sunday)
                startOfWeek = now.Date.AddDays(-6);

            var startOfLastWeek = startOfWeek.AddDays(-7);

            // ================== USER BY MONTH ==================
            var userByMonth = Enumerable.Range(1, 12)
                .Select(m =>
                    users.Count(u =>
                        u.CreatedAt.Year == now.Year &&
                        u.CreatedAt.Month == m
                    )
                )
                .ToList();

            // ================== USER BY WEEK (THIS WEEK) ==================
            var userByWeekThisWeek = Enumerable.Range(0, 7)
                .Select(i =>
                    users.Count(u =>
                        u.CreatedAt.Date == startOfWeek.AddDays(i)
                    )
                )
                .ToList();

            // ================== USER BY WEEK (LAST WEEK) ==================
            var userByWeekLastWeek = Enumerable.Range(0, 7)
                .Select(i =>
                    users.Count(u =>
                        u.CreatedAt.Date == startOfLastWeek.AddDays(i)
                    )
                )
                .ToList();

            // ================== RESPONSE ==================
            return new DashboardOverviewResponse
            {
                // ================== ORDERS ==================
                Orders = new OrderStatsDto
                {
                    TotalOrders = orders.Count,
                    Pending = orders.Count(o => o.StatusId == 1),
                    Confirmed = orders.Count(o => o.StatusId == 2),
                    Shipped = orders.Count(o => o.StatusId == 3),
                    Delivered = orders.Count(o => o.StatusId == 4),
                    Cancelled = orders.Count(o => o.StatusId == 5)
                },

                // ================== REVENUE ==================
                Revenue = new RevenueStatsDto
                {
                    TotalRevenue = orders
                        .Where(o => o.StatusId == 4)
                        .Sum(o => o.TotalAmount),

                    ThisMonthRevenue = orders
                        .Where(o =>
                            o.StatusId == 4 &&
                            o.CreatedAt >= startOfMonth)
                        .Sum(o => o.TotalAmount),

                    TodayRevenue = orders
                        .Where(o =>
                            o.StatusId == 4 &&
                            o.CreatedAt >= startOfToday)
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

                // ================== USERS ==================
                Users = new UserStatsDto
                {
                    TotalUsers = users.Count,
                    NewUsersThisMonth = users.Count(u => u.CreatedAt >= startOfMonth),

                    UserByMonth = userByMonth,
                    UserByWeekThisWeek = userByWeekThisWeek,
                    UserByWeekLastWeek = userByWeekLastWeek
                },

                // ================== PRODUCTS ==================
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
