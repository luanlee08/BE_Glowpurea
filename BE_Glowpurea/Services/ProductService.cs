using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.Helpers;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;
using BE_Glowpurea.Repositories;

namespace BE_Glowpurea.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _env;

        public ProductService(IProductRepository productRepo, IWebHostEnvironment env)
        {
            _productRepo = productRepo;
            _env = env;
        }

        public async Task<int> CreateAsync(CreateProductRequest request)
        {
            if (request.SubImages.Count != 6)
                throw new Exception("Phải upload đúng 6 ảnh phụ");

            if (await _productRepo.ExistsByNameAsync(request.ProductName))
            {
                throw new ArgumentException("Tên sản phẩm đã tồn tại");
            }

            if (request.Price < 0 || request.Price > 99_999_999)
            {
                throw new ArgumentException("Giá sản phẩm phải từ 0 đến 99.999.999");
            }

            // ❌ Số lượng không hợp lệ
            if (request.Quantity < 0 || request.Quantity > 99_999_999)
            {
                throw new ArgumentException("Số lượng phải từ 0 đến 99.999.999");
            }    
                
            var product = new Product
            {
                ProductName = request.ProductName,
                CategoryId = request.CategoryId,
                ShapesId = request.ShapesId,
                Price = request.Price,
                Quantity = request.Quantity,
                ProductStatus = request.ProductStatus,
                Description = request.Description,
                Sku = SkuGenerator.Generate(),
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            // Lưu ảnh
            product.ProductImages = SaveImages(request, product.Sku);

            await _productRepo.CreateAsync(product);
            return product.ProductId;
        }

        private List<ProductImage> SaveImages(CreateProductRequest request, string sku)
        {
            var images = new List<ProductImage>();
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "products", sku);

            Directory.CreateDirectory(uploadPath);

            // Main image
            var mainFileName = SaveFile(request.MainImage, uploadPath);
            images.Add(new ProductImage
            {
                ImageUrl = $"/uploads/products/{sku}/{mainFileName}",
                IsMain = true
            });

            // Sub images
            foreach (var file in request.SubImages)
            {
                var fileName = SaveFile(file, uploadPath);
                images.Add(new ProductImage
                {
                    ImageUrl = $"/uploads/products/{sku}/{fileName}",
                    IsMain = false
                });
            }

            return images;
        }

        private string SaveFile(IFormFile file, string path)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }

        public async Task<object> SearchAsync(SearchProductRequest request)
        {
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1) request.PageSize = 10;

            var (data, total) =
                await _productRepo.SearchByNameOrSkuAsync(request);

            return new
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Data = data
            };
        }

        public async Task UpdateAsync(int productId, UpdateProductRequest request)
        {
            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
                throw new ArgumentException("Sản phẩm không tồn tại");

            if (product.IsDeleted)
                throw new ArgumentException("Không thể chỉnh sửa sản phẩm đã bị xóa");

            // ❌ Trùng tên (trừ chính nó)
            if (await _productRepo.ExistsByNameExceptIdAsync(request.ProductName, productId))
                throw new ArgumentException("Tên sản phẩm đã tồn tại");

            // ❌ Giá không hợp lệ
            if (request.Price < 0 || request.Price > 99_999_999)
                throw new ArgumentException("Giá sản phẩm không hợp lệ");

            // ❌ Số lượng không hợp lệ
            if (request.Quantity < 0 || request.Quantity > 99_999_999)
                throw new ArgumentException("Số lượng không hợp lệ");

            // ✅ Update field
            product.ProductName = request.ProductName;
            product.CategoryId = request.CategoryId;
            product.ShapesId = request.ShapesId;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.ProductStatus = request.ProductStatus;
            product.Description = request.Description;
            product.UpdatedAt = DateTime.Now;

            await _productRepo.UpdateAsync(product);
        }

        public async Task<List<ProductListResponse>> GetAllAsync()
        {
            var products = await _productRepo.GetAllAsync();

            return products.Select(p => new ProductListResponse
            {
                ProductId = p.ProductId,
                Sku = p.Sku,
                ProductName = p.ProductName,
                CategoryName = p.Category != null ? p.Category.CategoryName : null, 
                ShapesName = p.Shapes != null ? p.Shapes.ShapesName : null,         
                Price = p.Price,
                Quantity = p.Quantity,
                ProductStatus = p.ProductStatus,
                CreatedAt = p.CreatedAt,                                            
                MainImageUrl = p.ProductImages
                    .Where(i => i.IsMain)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()                                            
            }).ToList();
        }


    }
}
