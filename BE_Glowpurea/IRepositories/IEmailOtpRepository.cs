using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface IEmailOtpRepository
    {
        Task CreateAsync(EmailOtp otp);
        Task<EmailOtp?> GetValidOtpAsync(string email, string otp, string purpose);
        Task MarkUsedAsync(EmailOtp otp);

        Task DeleteByAccountIdAsync(int accountId);
    }
}
