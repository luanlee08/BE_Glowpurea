using BE_Glowpurea.Dtos.Profile;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;

namespace BE_Glowpurea.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IWebHostEnvironment _env;

        public ProfileService(
            IAccountRepository accountRepo,
            IWebHostEnvironment env)
        {
            _accountRepo = accountRepo;
            _env = env;
        }

        public async Task<ProfileResponse> GetMyProfileAsync(int accountId)
        {
            var acc = await _accountRepo.GetByIdAsync(accountId);

            if (acc == null || acc.IsDeleted)
                throw new ArgumentException("Account không tồn tại");

            return new ProfileResponse
            {
                AccountId = acc.AccountId,
                AccountName = acc.AccountName,
                Email = acc.Email,
                PhoneNumber = acc.PhoneNumber,
                Image = acc.Image,
                Status = acc.Status,
                CreatedAt = acc.CreatedAt
            };
        }

        public async Task UpdateProfileAsync(int accountId, UpdateProfileRequest request)
        {
            var acc = await _accountRepo.GetByIdAsync(accountId);

            if (acc == null || acc.IsDeleted)
                throw new ArgumentException("Account không tồn tại");

            acc.AccountName = request.AccountName;
            acc.PhoneNumber = request.PhoneNumber;
            acc.UpdatedAt = DateTime.Now;

            await _accountRepo.UpdateAsync(acc);
        }

        public async Task<string> UploadAvatarAsync(int accountId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            var acc = await _accountRepo.GetByIdAsync(accountId);

            if (acc == null || acc.IsDeleted)
                throw new ArgumentException("Account không tồn tại");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"avatar_{accountId}{ext}";
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            acc.Image = $"/uploads/avatars/{fileName}";
            acc.UpdatedAt = DateTime.Now;

            await _accountRepo.UpdateAsync(acc);

            return acc.Image!;
        }
    }
}
