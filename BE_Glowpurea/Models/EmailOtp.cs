namespace BE_Glowpurea.Models
{
    public partial class EmailOtp
    {
        public int EmailOtpID { get; set; }

        public int? AccountID { get; set; }

        public string? Email { get; set; }

        public string Purpose { get; set; } = null!;
        // Register, ForgotPassword, ...

        public string OtpCode { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        // ===== Navigation =====
        public virtual Account? Account { get; set; }
    }
}
