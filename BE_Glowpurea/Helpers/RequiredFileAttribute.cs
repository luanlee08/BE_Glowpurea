using System.ComponentModel.DataAnnotations;

namespace BE_Glowpurea.Helpers
{
    public class RequiredFileAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null || file.Length == 0)
            {
                return new ValidationResult(ErrorMessage ?? "File là bắt buộc");
            }

            return ValidationResult.Success;
        }
    }

}
