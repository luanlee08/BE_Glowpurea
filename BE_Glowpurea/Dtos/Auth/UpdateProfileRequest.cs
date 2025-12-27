namespace BE_Glowpurea.Dtos.Auth
{
    public class UpdateProfileRequest
    {
        public string AccountName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
    }

}
