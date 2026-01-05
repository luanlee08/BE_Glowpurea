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
    }
}