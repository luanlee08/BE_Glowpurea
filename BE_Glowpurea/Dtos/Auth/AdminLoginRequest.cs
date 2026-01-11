namespace BE_Glowpurea.Dtos.Auth
{
    public class AdminLoginRequest
    {
        public string Identifier { get; set; } = null!; 
        public string Password { get; set; } = null!;
    }

}
