using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbGlowpureaContext _context;

        public CategoryRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
