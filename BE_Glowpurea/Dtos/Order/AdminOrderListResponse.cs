namespace BE_Glowpurea.Dtos.Order
{
    public class AdminOrderListResponse
    {
        public int OrderId { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
