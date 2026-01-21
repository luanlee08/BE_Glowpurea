namespace BE_Glowpurea.Dtos.Order
{
    public class OrderDetailResponse
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
