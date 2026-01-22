namespace BE_Glowpurea.Dtos.Order
{
    public class OrderListResponse
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        // PREVIEW
        public int TotalItems { get; set; }
        public OrderItemPreviewResponse? PreviewItem { get; set; }
    }
}
