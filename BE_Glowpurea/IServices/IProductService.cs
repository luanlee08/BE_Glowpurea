using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Product;

namespace BE_Glowpurea.IServices
{
    public interface IProductService
    {
        Task<PagedResponse<ProductListResponse>>GetPagedAsync(SearchProductRequest request);

        Task<int> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(int productId, UpdateProductRequest request);
    }
}
