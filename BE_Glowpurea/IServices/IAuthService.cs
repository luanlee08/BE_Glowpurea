using BE_Glowpurea.Dtos.Auth;

namespace BE_Glowpurea.IServices
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task VerifyOtpAsync(VerifyOtpRequest request);
    }
}
