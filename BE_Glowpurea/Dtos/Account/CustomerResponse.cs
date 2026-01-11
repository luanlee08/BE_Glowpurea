namespace BE_Glowpurea.Dtos.Account
{
    public class CustomerResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
