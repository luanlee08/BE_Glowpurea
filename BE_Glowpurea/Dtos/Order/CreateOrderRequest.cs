namespace BE_Glowpurea.Dtos.Order
{
    public class CreateOrderRequest
    {
        public string ShippingName { get; set; } = null!;
        public string ShippingPhone { get; set; } = null!;
        public string ShippingAddressLine { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingWard { get; set; } = null!;
        public string ShippingMethod { get; set; } = "COD";

        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
