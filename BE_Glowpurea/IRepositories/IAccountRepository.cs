using BE_Glowpurea.Dtos.Account;
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
        //admin
        Task<(List<AccountResponse> Data, int Total)> SearchAsync(SearchAccountRequest request);
        Task<Account?> GetByIdAsync(int accountId);
        Task<AccountDetailResponse?> GetDetailByIdAsync(int accountId);
        Task<(List<AccountResponse> Data, int Total)> GetAllAsync(int page, int pageSize);


        Task<(List<CustomerResponse> Data, int Total)>
        SearchCustomerAsync(SearchAccountRequest request);

        Task<Account?> GetCustomerByIdAsync(int customerId);

    }
}
