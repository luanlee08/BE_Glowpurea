using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}
