using BE_Glowpurea.Dtos.Account;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/accounts")]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers(
            [FromQuery] SearchAccountRequest request)
        {
            var result = await _accountService.SearchAsync(request);
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateCustomerStatusRequest request)
        {
            await _accountService.UpdateCustomerStatusAsync(id, request);

            return Ok(new
            {
                Message = "Cập nhật trạng thái customer thành công"
            });
        }

    }
}
