using BE_Glowpurea.Dtos.Account;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;

namespace BE_Glowpurea.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<object> SearchAsync(SearchAccountRequest request)
        {
            var (data, total) = await _accountRepo.SearchAsync(request);

            return new
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Data = data
            };
        }

        public async Task UpdateAsync(int accountId, UpdateAccountRequest request)
        {
            var account = await _accountRepo.GetByIdAsync(accountId);

            if (account == null)
                throw new ArgumentException("Account không tồn tại");

            if (account.IsDeleted)
                throw new ArgumentException("Không thể chỉnh sửa account đã bị xóa");

            // update field
            account.AccountName = request.AccountName;
            account.PhoneNumber = request.PhoneNumber;
            account.Status = request.Status;
            account.RoleId = request.RoleId;
            account.Image = request.Image;
            account.UpdatedAt = DateTime.Now;

            await _accountRepo.UpdateAsync(account);
        }

        public async Task<AccountDetailResponse> GetByIdAsync(int accountId)
        {
            var account = await _accountRepo.GetDetailByIdAsync(accountId);

            if (account == null)
                throw new ArgumentException("Account không tồn tại");

            return account;
        }

        public async Task<object> GetAllAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var (data, total) = await _accountRepo.GetAllAsync(page, pageSize);

            return new
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Data = data
            };
        }
        public async Task<object> SearchCustomerAsync(
          SearchAccountRequest request)
        {
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1) request.PageSize = 10;

            request.RoleId = 2; // Customer

            var (data, total) =
                await _accountRepo.SearchCustomerAsync(request);

            return new
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Data = data
            };
        }

        // ================= ADMIN: BLOCK / UNBLOCK =================
        public async Task UpdateCustomerStatusAsync(
            int customerId,
            UpdateCustomerStatusRequest request)
        {
            var customer = await _accountRepo.GetCustomerByIdAsync(customerId);

            if (customer == null)
                throw new ArgumentException("Customer không tồn tại");

            customer.Status = request.Status; // Active | Blocked
            customer.UpdatedAt = DateTime.Now;

            await _accountRepo.UpdateAsync(customer);
        }

    }
}
