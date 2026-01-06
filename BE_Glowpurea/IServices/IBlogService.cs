using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Blog;

namespace BE_Glowpurea.IServices
{
    public interface IBlogService
    {
        Task<int> CreateAsync(CreateBlogRequest request, int accountId);

        Task<PagedResponse<BlogAdminResponse>>
            SearchForAdminAsync(SearchBlogAdminRequest request);
        Task UpdateAsync(int blogId, UpdateBlogRequest request);

        Task<PagedResponse<BlogPublicResponse>> GetPublicAsync(
            string? keyword,
            int? categoryId,
            int page,
            int pageSize
        );

        Task<BlogDetailResponse> GetPublicDetailAsync(int blogId);

        Task<List<BlogPublicResponse>> GetRecentAsync(int limit);

    }
}