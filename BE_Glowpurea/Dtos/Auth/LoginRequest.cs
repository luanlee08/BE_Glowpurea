namespace BE_Glowpurea.Dtos.Auth
{
    public class LoginRequest
    {
        public string Identifier { get; set; } = null!; // email OR phone
        public string Password { get; set; } = null!;

    }
}
