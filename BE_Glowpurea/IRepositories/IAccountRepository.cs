using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByPhoneAsync(string phone);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phone);
        Task CreateAsync(Account account);

        Task UpdateAsync(Account account);
    }
}
