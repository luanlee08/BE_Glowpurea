using BE_Glowpurea.Dtos.Account;

namespace BE_Glowpurea.IServices
{
    public interface IAccountService
    {
        Task<object> SearchAsync(SearchAccountRequest request);
        Task UpdateAsync(int accountId, UpdateAccountRequest request);
        Task<AccountDetailResponse> GetByIdAsync(int accountId);
        Task<object> GetAllAsync(int page, int pageSize);

        Task<object> SearchCustomerAsync(SearchAccountRequest request);
        Task UpdateCustomerStatusAsync(
            int customerId,
            UpdateCustomerStatusRequest request);

    }
}
