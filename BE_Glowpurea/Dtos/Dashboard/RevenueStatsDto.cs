namespace BE_Glowpurea.Dtos.Dashboard
{
    public class RevenueStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal ThisMonthRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public List<decimal> MonthlyRevenue { get; set; } = new();
    }
}
