using BE_Glowpurea.Models;

namespace BE_Glowpurea.IServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
    }
}
