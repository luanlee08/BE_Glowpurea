using System.ComponentModel.DataAnnotations;

namespace BE_Glowpurea.Dtos.Product
{
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255)]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "CategoryId không được để trống")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "ShapesId không được để trống")]
        public int? ShapesId { get; set; }

        [Range(0, 99_999_999, ErrorMessage = "Giá phải từ 0 đến 99.999.999")]
        public decimal Price { get; set; }

        [Range(0, 99_999_999, ErrorMessage = "Số lượng phải từ 0 đến 99.999.999")]
        public int Quantity { get; set; }

        [Required]
        [RegularExpression(
            "^(Available|OutOfStock|Discontinued)$",
            ErrorMessage = "ProductStatus không hợp lệ"
        )]
        public string ProductStatus { get; set; } = null!;

        public string? Description { get; set; }
    }
}
