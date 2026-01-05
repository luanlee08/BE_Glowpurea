using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IBlogCategoryRepository
    {
        Task<List<BlogCategory>> GetAllAsync();
        Task<List<BlogCategory>> GetByIdsAsync(List<int> ids);
    }
}
