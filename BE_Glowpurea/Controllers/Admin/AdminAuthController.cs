using BE_Glowpurea.IServices;
using BE_Glowpurea.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BE_Glowpurea.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/auth")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AdminAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequest request)
        {
            var result = await _authService.AdminLoginAsync(request);
            return Ok(result);
        }

      
    }
}
