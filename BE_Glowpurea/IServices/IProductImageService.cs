namespace BE_Glowpurea.IServices
{
    public interface IProductImageService
    {
        Task SaveImagesAsync(
        int productId,
        string sku,
        IFormFile mainImage,
        List<IFormFile> subImages);

        Task UpdateImagesAsync(
     int productId,
     string sku,
     IFormFile? newMainImage,
     List<IFormFile>? newSubImages,
     List<string>? keepSubImageUrls
 );

    }
}
