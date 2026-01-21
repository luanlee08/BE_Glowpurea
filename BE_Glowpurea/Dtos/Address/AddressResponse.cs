namespace BE_Glowpurea.Dtos.Address
{
    public class AddressResponse
    {
        public int AddressID { get; set; }
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Ward { get; set; }
        public bool IsDefault { get; set; }
    }
}
