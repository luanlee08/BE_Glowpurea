using System.ComponentModel.DataAnnotations;

namespace BE_Glowpurea.Dtos.Account
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = "AccountName không được để trống")]
        [StringLength(100)]
        public string AccountName { get; set; } = null!;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        public int? RoleId { get; set; }

        public string? Image { get; set; }
    }
}
