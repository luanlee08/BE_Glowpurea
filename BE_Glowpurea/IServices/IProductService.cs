using BE_Glowpurea.Dtos.Product;

namespace BE_Glowpurea.IServices
{
    public interface IProductService
    {
        Task<int> CreateAsync(CreateProductRequest request);
        Task<object> SearchAsync(SearchProductRequest request);
        Task UpdateAsync(int productId, UpdateProductRequest request);

    }
}
