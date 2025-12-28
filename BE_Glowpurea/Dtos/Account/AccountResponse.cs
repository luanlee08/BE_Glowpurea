namespace BE_Glowpurea.Dtos.Account
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Status { get; set; } = null!;
        public string? RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
