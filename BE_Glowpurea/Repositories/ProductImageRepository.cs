using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly DbGlowpureaContext _ctx;

        public ProductImageRepository(DbGlowpureaContext ctx)
        {
            _ctx = ctx;
        }

        public Task<List<ProductImage>> GetByProductIdAsync(int productId)
            => _ctx.ProductImages
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.IsMain)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();

        public Task<ProductImage?> GetMainAsync(int productId)
            => _ctx.ProductImages
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.IsMain);

        public Task<int> CountSecondaryAsync(int productId)
            => _ctx.ProductImages
                .CountAsync(x => x.ProductId == productId && !x.IsMain);

        public async Task UnsetMainAsync(int productId)
        {
            var mains = await _ctx.ProductImages
                .Where(x => x.ProductId == productId && x.IsMain)
                .ToListAsync();

            foreach (var m in mains)
                m.IsMain = false;
        }

        public Task AddAsync(ProductImage entity)
            => _ctx.ProductImages.AddAsync(entity).AsTask();

        public Task AddRangeAsync(IEnumerable<ProductImage> entities)
            => _ctx.ProductImages.AddRangeAsync(entities);

        public void RemoveRange(IEnumerable<ProductImage> entities)
            => _ctx.ProductImages.RemoveRange(entities);

        public Task SaveChangesAsync()
            => _ctx.SaveChangesAsync();

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await using var tx = await _ctx.Database.BeginTransactionAsync();
            try
            {
                await action();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
        public async Task RemoveSecondaryAsync(int productId)
        {
            var subs = await _ctx.ProductImages
                .Where(x => x.ProductId == productId && !x.IsMain)
                .ToListAsync();

            _ctx.ProductImages.RemoveRange(subs);
        }
        public Task<List<ProductImage>> GetSecondaryAsync(int productId)
        {
            return _ctx.ProductImages
                .Where(x => x.ProductId == productId && !x.IsMain)
                .ToListAsync();
        }

    }
}
