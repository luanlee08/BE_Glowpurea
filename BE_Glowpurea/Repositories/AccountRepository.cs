using BE_Glowpurea.Dtos.Account;
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

        //=====================================================Admin============================================================
        public async Task<(List<AccountResponse> Data, int Total)> SearchAsync(SearchAccountRequest request)
        {
            var query = _context.Accounts
                .Include(a => a.Role)
                .Where(a => !a.IsDeleted)
                .AsQueryable();

            // 🔎 Keyword: name / email / phone
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();
                query = query.Where(a =>
                    a.AccountName.Contains(keyword) ||
                    a.Email.Contains(keyword) ||
                    a.PhoneNumber!.Contains(keyword));
            }

            // 👤 Role
            if (request.RoleId.HasValue)
                query = query.Where(a => a.RoleId == request.RoleId);

            // 🟢 Status
            if (!string.IsNullOrEmpty(request.Status))
                query = query.Where(a => a.Status == request.Status);

            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AccountResponse
                {
                    AccountId = a.AccountId,
                    AccountName = a.AccountName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Status = a.Status,
                    RoleName = a.Role != null ? a.Role.RoleName : null,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return (data, total);
        }

        public async Task<Account?> GetByIdAsync(int accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<AccountDetailResponse?> GetDetailByIdAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Role)
                .Where(a => !a.IsDeleted && a.AccountId == accountId)
                .Select(a => new AccountDetailResponse
                {
                    AccountId = a.AccountId,
                    AccountName = a.AccountName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Status = a.Status,
                    Image = a.Image,
                    RoleName = a.Role != null ? a.Role.RoleName : null,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Provider = a.Provider
                })
                .FirstOrDefaultAsync();
        }
        public async Task<(List<AccountResponse> Data, int Total)>
            GetAllAsync(int page, int pageSize)
        {
            var query = _context.Accounts
                .Include(a => a.Role)
                .Where(a => !a.IsDeleted)
                .AsQueryable();

            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AccountResponse
                {
                    AccountId = a.AccountId,
                    AccountName = a.AccountName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Status = a.Status,
                    RoleName = a.Role != null ? a.Role.RoleName : null,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return (data, total);
        }

    }
}
