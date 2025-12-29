using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IProductRepository
    {
        Task<(List<ProductListResponse> Data, int Total)> SearchByNameOrSkuAsync(SearchProductRequest request);
        Task<bool> ExistsByNameAsync(string productName);
        Task<Product> CreateAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int productId);
        Task<bool> ExistsByNameExceptIdAsync(string productName, int productId);
        Task UpdateAsync(Product product);

    }
}
