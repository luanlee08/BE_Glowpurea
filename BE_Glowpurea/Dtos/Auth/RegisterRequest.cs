using System.ComponentModel.DataAnnotations;

namespace BE_Glowpurea.Dtos.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100)]
        public string AccountName { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; } = null!;
    }
}
