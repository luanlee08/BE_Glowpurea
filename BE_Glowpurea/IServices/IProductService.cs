using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Product;

namespace BE_Glowpurea.IServices
{
    public interface IProductService
    {
        Task<PagedResponse<ProductListResponse>>GetPagedAsync(SearchProductRequest request);
        Task<ProductDetailResponse?> GetByIdAsync(int productId);
        Task<int> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(int productId, UpdateProductRequest request);
        Task<List<UserProductCardResponse>> GetForUserAsync();
        Task<PagedResponse<UserProductCardResponse>>
            GetForUserPagedAsync(UserProductPagingRequest request);
    }
}
