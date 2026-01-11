using BE_Glowpurea.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BE_Glowpurea.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        // ================= CUSTOMER / USER =================
        public string GenerateToken(Account account, string roleName)
        {
            return GenerateToken(account, roleName, out _);
        }

        // ================= ADMIN (SINGLE LOGIN) =================
        public string GenerateToken(
            Account account,
            string roleName,
            out string jti
        )
        {
            jti = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("AccountId", account.AccountId.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    int.Parse(_config["Jwt:ExpireMinutes"]!)
                ),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
