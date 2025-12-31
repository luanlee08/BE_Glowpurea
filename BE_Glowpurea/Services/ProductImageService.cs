using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.Services
{
    public class ProductImageService : IProductImageService
    {
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
            if (subImages.Count != MAX_SUB)
                throw new ArgumentException("Phải upload đúng 6 ảnh phụ");

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
                // MAIN
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

                // SUB
                if (newSubImages != null && newSubImages.Any())
                {
                    var existed = await _repo.CountSecondaryAsync(productId);
                    var slots = Math.Max(0, MAX_SUB - existed);

                    var toAdd = newSubImages.Take(slots)
                        .Select(f => new ProductImage
                        {
                            ProductId = productId,
                            ImageUrl = SaveFile(f, uploadPath, sku),
                            IsMain = false
                        });

                    await _repo.AddRangeAsync(toAdd);
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
