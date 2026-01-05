using BE_Glowpurea.Dtos.Blog;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;

namespace BE_Glowpurea.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _repo;

        public BlogCategoryService(IBlogCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<BlogCategoryResponse>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            return categories.Select(c => new BlogCategoryResponse
            {
                BlogCategoryId = c.BlogCategoryId,
                BlogCategoryName = c.BlogCategoryName
            }).ToList();
        }
    }
}
