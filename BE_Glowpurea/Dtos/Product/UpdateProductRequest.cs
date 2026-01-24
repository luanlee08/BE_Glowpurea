using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BE_Glowpurea.Dtos.Product
{
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm tối đa 255 ký tự")]
        public string ProductName { get; set; } = null!;

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public int? ShapesId { get; set; }

        [Range(0, 99_999_999)]
        public decimal Price { get; set; }

        [Range(0, 99_999_999)]
        public int Quantity { get; set; }

        [Required]
        [RegularExpression("^(Available|OutOfStock|Discontinued)$")]
        public string ProductStatus { get; set; } = null!;

        public string? Description { get; set; }

        // ✅ THÊM ẢNH UPDATE
        public IFormFile? NewMainImage { get; set; }
        public List<IFormFile>? NewSubImages { get; set; }
        public List<string>? KeepSubImageUrls { get; set; }

    }
}
