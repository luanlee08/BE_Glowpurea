namespace BE_Glowpurea.Dtos.Product
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Sku { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductStatus { get; set; } = null!;
        public string? MainImageUrl { get; set; }
    }
}
