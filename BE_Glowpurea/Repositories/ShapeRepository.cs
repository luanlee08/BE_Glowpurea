using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class ShapeRepository : IShapeRepository
    {
        private readonly DbGlowpureaContext _context;

        public ShapeRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<List<Shape>> GetAllAsync()
        {
            return await _context.Shapes
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}
