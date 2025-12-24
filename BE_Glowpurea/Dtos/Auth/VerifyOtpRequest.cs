namespace BE_Glowpurea.Dtos.Auth
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
    }
}
