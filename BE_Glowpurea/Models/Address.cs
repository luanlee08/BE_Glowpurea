namespace BE_Glowpurea.Models
{
    public partial class Address
    {
        public int AddressID { get; set; }

        public int AccountID { get; set; }

        public string AddressLine { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? Ward { get; set; }

        public bool IsDefault { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // ===== Navigation =====
        public virtual Account Account { get; set; } = null!;
    }
}
