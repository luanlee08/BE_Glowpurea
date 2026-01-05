namespace BE_Glowpurea.Dtos.Product
{
    public class ProductDetailResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public int? ShapesId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductStatus { get; set; } = null!;
        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }
        public List<string> SubImageUrls { get; set; } = new();
    }
}
