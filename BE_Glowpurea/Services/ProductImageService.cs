using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.Services
{
    public class ProductImageService : IProductImageService
    {
        private const int MIN_SUB = 4;
        private const int MAX_SUB = 6;
        private readonly IProductImageRepository _repo;
        private readonly IWebHostEnvironment _env;

        public ProductImageService(
            IProductImageRepository repo,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task SaveImagesAsync(
            int productId,
            string sku,
            IFormFile mainImage,
            List<IFormFile> subImages)
        {
            if (subImages.Count < MIN_SUB || subImages.Count > MAX_SUB)
                throw new ArgumentException("Ảnh phụ phải từ 4 đến 6 ảnh");

            var uploadPath = Path.Combine(
                _env.WebRootPath, "uploads", "products", sku);

            Directory.CreateDirectory(uploadPath);

            await _repo.ExecuteInTransactionAsync(async () =>
            {
                // MAIN
                var mainUrl = SaveFile(mainImage, uploadPath, sku);
                await _repo.AddAsync(new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = mainUrl,
                    IsMain = true
                });

                // SUB
                var subs = subImages.Select(f => new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = SaveFile(f, uploadPath, sku),
                    IsMain = false
                });

                await _repo.AddRangeAsync(subs);
                await _repo.SaveChangesAsync();
            });
        }

        public async Task UpdateImagesAsync(
     int productId,
     string sku,
     IFormFile? newMainImage,
     List<IFormFile>? newSubImages)
        {
            var uploadPath = Path.Combine(
                _env.WebRootPath, "uploads", "products", sku);

            Directory.CreateDirectory(uploadPath);

            await _repo.ExecuteInTransactionAsync(async () =>
            {
                // ================= MAIN IMAGE =================
                if (newMainImage != null)
                {
                    await _repo.UnsetMainAsync(productId);

                    var mainUrl = SaveFile(newMainImage, uploadPath, sku);
                    await _repo.AddAsync(new ProductImage
                    {
                        ProductId = productId,
                        ImageUrl = mainUrl,
                        IsMain = true
                    });
                }

                // ================= SUB IMAGES (REPLACE) =================
                if (newSubImages != null && newSubImages.Any())
                {
                    // 1️⃣ Validate theo ảnh MỚI
                    if (newSubImages.Count < MIN_SUB || newSubImages.Count > MAX_SUB)
                        throw new ArgumentException(
                            $"Ảnh phụ phải từ {MIN_SUB} đến {MAX_SUB}");

                    // 2️⃣ XOÁ TOÀN BỘ ảnh phụ cũ
                    await _repo.RemoveSecondaryAsync(productId);

                    // 3️⃣ ADD ảnh phụ mới
                    var subs = newSubImages.Select(f => new ProductImage
                    {
                        ProductId = productId,
                        ImageUrl = SaveFile(f, uploadPath, sku),
                        IsMain = false
                    });

                    await _repo.AddRangeAsync(subs);
                }

                await _repo.SaveChangesAsync();
            });
        }

        private string SaveFile(IFormFile file, string path, string sku)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            return $"/uploads/products/{sku}/{fileName}";
        }
    }
}
