using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IRoleRepository
    {
        Task<Role> GetCustomerRoleAsync();
    }
}
