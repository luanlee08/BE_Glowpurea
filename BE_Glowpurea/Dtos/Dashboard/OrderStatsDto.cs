namespace BE_Glowpurea.Dtos.Dashboard
{
    public class OrderStatsDto
    {
        public int TotalOrders { get; set; }
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int Shipped { get; set; }
        public int Delivered { get; set; }
        public int Cancelled { get; set; }
    }
}
