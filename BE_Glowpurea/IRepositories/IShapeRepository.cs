using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IShapeRepository
    {
        Task<List<Shape>> GetAllAsync();
    }
}
