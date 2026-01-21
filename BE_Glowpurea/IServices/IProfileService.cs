using BE_Glowpurea.Dtos.Profile;

namespace BE_Glowpurea.IServices
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetMyProfileAsync(int accountId);
        Task UpdateProfileAsync(int accountId, UpdateProfileRequest request);
        Task<string> UploadAvatarAsync(int accountId, IFormFile file);
    }
}
