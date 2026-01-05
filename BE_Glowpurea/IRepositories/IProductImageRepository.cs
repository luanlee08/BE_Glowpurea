using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IProductImageRepository
    {
        Task<List<ProductImage>> GetByProductIdAsync(int productId);
        Task<ProductImage?> GetMainAsync(int productId);
        Task<int> CountSecondaryAsync(int productId);

        Task UnsetMainAsync(int productId);

        Task AddAsync(ProductImage entity);
        Task AddRangeAsync(IEnumerable<ProductImage> entities);
        void RemoveRange(IEnumerable<ProductImage> entities);
        Task RemoveSecondaryAsync(int productId);

        Task SaveChangesAsync();
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
