using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.Services
{
    public class ShapeService : IShapeService
    {
        private readonly IShapeRepository _shapeRepository;

        public ShapeService(IShapeRepository shapeRepository)
        {
            _shapeRepository = shapeRepository;
        }

        public async Task<List<Shape>> GetAllAsync()
        {
            return await _shapeRepository.GetAllAsync();
        }
    }
}
