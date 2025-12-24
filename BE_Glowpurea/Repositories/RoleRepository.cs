using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbGlowpureaContext _context;

        public RoleRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<Role> GetCustomerRoleAsync()
            => await _context.Roles.FirstAsync(r => r.RoleName == "customer");
    }
}
