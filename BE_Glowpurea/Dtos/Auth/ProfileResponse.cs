namespace BE_Glowpurea.Dtos.Auth
{
    public class ProfileResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = null!;
        public string? Image { get; set; }   
    }

}
