using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IBlogRepository
    {
        Task AddAsync(BlogPost blog);

        Task<(List<BlogPost> Data, int Total)> SearchForAdminAsync(string? keyword, int page, int pageSize);
        Task<BlogPost?> GetByIdAsync(int blogId);

        Task SaveChangesAsync();
    }
}
