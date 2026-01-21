namespace BE_Glowpurea.Dtos.Address
{
    public class CreateAddressRequest
    {
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Ward { get; set; }
        public bool IsDefault { get; set; }
    }
}
