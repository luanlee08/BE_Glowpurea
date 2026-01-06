namespace BE_Glowpurea.Dtos.Product
{
    public class UserProductCardResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Ingredients { get; set; } = null!;
    }
}
