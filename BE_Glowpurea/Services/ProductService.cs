using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.Helpers;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductImageService _imageService;

        public ProductService(
            IProductRepository productRepo,
            IProductImageService imageService)
        {
            _productRepo = productRepo;
            _imageService = imageService;
        }

        /* ===================== GET PAGED ===================== */

        public async Task<PagedResponse<ProductListResponse>> GetPagedAsync(SearchProductRequest request)
        {
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1) request.PageSize = 10;

            var (data, total) = await _productRepo.GetPagedAsync(request);

            return new PagedResponse<ProductListResponse>
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Data = data
            };
        }

        /* ===================== CREATE ===================== */

        public async Task<int> CreateAsync(CreateProductRequest request)
        {
            if (request.SubImages.Count < 4 || request.SubImages.Count > 6)
                throw new ArgumentException("Ảnh phụ phải từ 4 đến 6 ảnh");

            if (await _productRepo.ExistsByNameAsync(request.ProductName))
                throw new ArgumentException("Tên sản phẩm đã tồn tại");

            if (request.Price < 0 || request.Price > 99_999_999)
                throw new ArgumentException("Giá sản phẩm không hợp lệ");

            if (request.Quantity < 0 || request.Quantity > 99_999_999)
                throw new ArgumentException("Số lượng không hợp lệ");

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

            // 1️⃣ Lưu product trước
            await _productRepo.CreateAsync(product);

            // 2️⃣ Giao ảnh cho ImageService
            await _imageService.SaveImagesAsync(
                product.ProductId,
                product.Sku,
                request.MainImage,
                request.SubImages
            );

            return product.ProductId;
        }

        /* ===================== UPDATE ===================== */

        public async Task UpdateAsync(int productId, UpdateProductRequest request)
        {
            var product = await _productRepo.GetByIdAsync(productId)
                ?? throw new ArgumentException("Sản phẩm không tồn tại");

            if (product.IsDeleted)
                throw new ArgumentException("Không thể chỉnh sửa sản phẩm đã bị xóa");

            if (await _productRepo.ExistsByNameExceptIdAsync(request.ProductName, productId))
                throw new ArgumentException("Tên sản phẩm đã tồn tại");

            if (request.Price < 0 || request.Price > 99_999_999)
                throw new ArgumentException("Giá sản phẩm không hợp lệ");

            if (request.Quantity < 0 || request.Quantity > 99_999_999)
                throw new ArgumentException("Số lượng không hợp lệ");

            product.ProductName = request.ProductName;
            product.CategoryId = request.CategoryId;
            product.ShapesId = request.ShapesId;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.ProductStatus = request.ProductStatus;
            product.Description = request.Description;
            product.UpdatedAt = DateTime.Now;

            await _productRepo.UpdateAsync(product);

            // ✅ Update ảnh nếu có
            if (request.NewMainImage != null || request.NewSubImages?.Any() == true)
            {
                await _imageService.UpdateImagesAsync(
                    product.ProductId,
                    product.Sku,
                    request.NewMainImage,
                    request.NewSubImages
                );
            }
        }


        public async Task<ProductDetailResponse?> GetByIdAsync(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null || product.IsDeleted)
                return null;

            return new ProductDetailResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                ShapesId = product.ShapesId,
                Price = product.Price,
                Quantity = product.Quantity,
                ProductStatus = product.ProductStatus,
                Description = product.Description,
                MainImageUrl = product.ProductImages
                    .FirstOrDefault(x => x.IsMain)?.ImageUrl,
                SubImageUrls = product.ProductImages
                    .Where(x => !x.IsMain)
                    .Select(x => x.ImageUrl)
                    .ToList()
            };
        }

        public async Task<List<UserProductCardResponse>> GetForUserAsync()
        {
            return await _productRepo.GetForUserAsync();
        }

        public async Task<PagedResponse<UserProductCardResponse>>
    GetForUserPagedAsync(UserProductPagingRequest request)
        {
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1) request.PageSize = 8;

            var (data, total) =
                await _productRepo.GetForUserPagedAsync(request);

            return new PagedResponse<UserProductCardResponse>
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Data = data
            };
        }

    }
}
