namespace BE_Glowpurea.Dtos.Auth
{
    public class RegisterRequest
    {
        public string AccountName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = null!;
    }
}
