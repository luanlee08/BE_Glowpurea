using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class BlogCategoryRepository : IBlogCategoryRepository
    {
        private readonly DbGlowpureaContext _context;

        public BlogCategoryRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<List<BlogCategory>> GetAllAsync()
        {
            return await _context.BlogCategories
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<BlogCategory>> GetByIdsAsync(List<int> ids)
        {
            return await _context.BlogCategories
                .Where(c => ids.Contains(c.BlogCategoryId) && !c.IsDeleted)
                .ToListAsync();
        }
    }
}
