namespace BE_Glowpurea.Dtos.Profile
{
    public class ProfileResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

    }
}
