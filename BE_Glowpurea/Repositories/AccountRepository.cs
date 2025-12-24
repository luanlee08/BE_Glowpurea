using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbGlowpureaContext _context;

        public AccountRepository(DbGlowpureaContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
         => await _context.Accounts.AnyAsync(a => a.Email == email);

        public async Task<Account?> GetByEmailAsync(string email)
               => await _context.Accounts
                   .Include(a => a.Role)
                   .FirstOrDefaultAsync(a => a.Email == email);

        public async Task<Account?> GetByPhoneAsync(string phone)
            => await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.PhoneNumber == phone);

        public async Task<bool> PhoneExistsAsync(string phone)
            => await _context.Accounts.AnyAsync(a => a.PhoneNumber == phone);
        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

    }
}
