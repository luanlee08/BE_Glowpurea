namespace BE_Glowpurea.Dtos.Product
{
    public class ProductListResponse
    {
        public int ProductId { get; set; }
        public string? MainImageUrl { get; set; }
        public string Sku { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string? ShapesName { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

}
