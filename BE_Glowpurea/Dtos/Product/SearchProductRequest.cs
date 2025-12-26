namespace BE_Glowpurea.Dtos.Product
{
    public class SearchProductRequest
    {
        public string? Keyword { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
