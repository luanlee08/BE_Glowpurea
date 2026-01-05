using System.ComponentModel.DataAnnotations;

namespace BE_Glowpurea.Dtos.Product
{
        public class CreateProductRequest
        {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm tối đa 255 ký tự")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "CategoryId không được để trống")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "ShapesId không được để trống")]
        public int? ShapesId { get; set; }

        [Range(1000, 10000000, ErrorMessage = "Giá sản phẩm phải từ 1.000 đến 10.000.000")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "ProductStatus không được để trống")]
        [RegularExpression(
            "^(Available|OutOfStock|Discontinued)$",
            ErrorMessage = "ProductStatus chỉ được là: Available, OutOfStock, Discontinued"
        )]
        public string ProductStatus { get; set; } = null!;

        public string? Description { get; set; }

        // Ảnh
        [Required(ErrorMessage = "Ảnh chính (MainImage) là bắt buộc")]
        public IFormFile MainImage { get; set; } = null!;

        [MinLength(4, ErrorMessage = "Phải upload ít nhất 4 ảnh phụ")]
        public List<IFormFile> SubImages { get; set; } = new();
    }
}
