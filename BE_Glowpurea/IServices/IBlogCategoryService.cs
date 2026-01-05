using BE_Glowpurea.Dtos.Blog;

namespace BE_Glowpurea.IServices
{
    public interface IBlogCategoryService
    {
        Task<List<BlogCategoryResponse>> GetAllAsync();
    }
}
