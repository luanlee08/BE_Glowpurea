using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class EmailOtpRepository : IEmailOtpRepository
    {
        private readonly DbGlowpureaContext _context;

        public EmailOtpRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(EmailOtp otp)
        {
            _context.EmailOtps.Add(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailOtp?> GetValidOtpAsync(string email, string otp, string purpose)
        {
            return await _context.EmailOtps.FirstOrDefaultAsync(x =>
                x.Email == email &&
                x.OtpCode == otp &&
                x.Purpose == purpose &&
                !x.IsUsed &&
                x.ExpiresAt > DateTime.Now
            );
        }

        public async Task MarkUsedAsync(EmailOtp otp)
        {
            otp.IsUsed = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByAccountIdAsync(int accountId)
        {
            var otps = _context.EmailOtps
                .Where(x => x.AccountID == accountId);

            _context.EmailOtps.RemoveRange(otps);
            await _context.SaveChangesAsync();
        }

    }
}
