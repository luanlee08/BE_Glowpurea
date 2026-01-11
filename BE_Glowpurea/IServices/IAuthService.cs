using BE_Glowpurea.Dtos.Auth;

namespace BE_Glowpurea.IServices
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task VerifyOtpAsync(VerifyOtpRequest request);

        Task<ProfileResponse> GetProfileAsync(string email);
        Task<string> UploadAvatarAsync(string email, IFormFile image);
        Task<ProfileResponse> UpdateProfileAsync(string email, UpdateProfileRequest request);
        Task<AdminLoginResponse> AdminLoginAsync(AdminLoginRequest request);
    }
}
