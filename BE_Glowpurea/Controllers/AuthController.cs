using System.Security.Claims;
using BE_Glowpurea.Dtos.Auth;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .First().ErrorMessage;

                return BadRequest(error);
            }

            try
            {
                await _authService.RegisterAsync(request);
                return Ok(new { message = "Đã gửi OTP qua email" });
            }
            catch (Exception ex)
            {
                // ✅ CHỈ TRẢ MESSAGE, KHÔNG TRẢ STACK TRACE
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                await _authService.VerifyOtpAsync(request);
                return Ok("Xác thực thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
          
            try
            {
                return Ok(await _authService.LoginAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // JWT stateless → không cần xử lý gì
            return Ok(new { message = "Đăng xuất thành công" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();

            return Ok(await _authService.GetProfileAsync(email));
        }

        [Authorize]
        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();

            var imageUrl = await _authService.UploadAvatarAsync(email, request.Image);
            return Ok(new { image = imageUrl });
        }

    }

}

