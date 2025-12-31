using BE_Glowpurea.Models;

namespace BE_Glowpurea.IServices
{
    public interface IShapeService
    {
        Task<List<Shape>> GetAllAsync();
    }
}
