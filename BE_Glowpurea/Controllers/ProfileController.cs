using BE_Glowpurea.Dtos.Profile;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private int AccountId =>
            int.Parse(User.FindFirst("AccountId")!.Value);

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _profileService.GetMyProfileAsync(AccountId);
            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateProfileRequest request)
        {
            await _profileService.UpdateProfileAsync(AccountId, request);
            return Ok(new { message = "Cập nhật profile thành công" });
        }

        [HttpPost("avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var url = await _profileService.UploadAvatarAsync(AccountId, file);
            return Ok(new { imageUrl = url });
        }
    }
}
