namespace BE_Glowpurea.Dtos.Dashboard
{
    public class DashboardOverviewResponse
    {
        public OrderStatsDto Orders { get; set; } = null!;
        public RevenueStatsDto Revenue { get; set; } = null!;
        public UserStatsDto Users { get; set; } = null!;
        public ProductStatsDto Products { get; set; } = null!;
    }

}
