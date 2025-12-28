using BE_Glowpurea.Dtos.Account;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchAccountRequest request)
        {
            var result = await _accountService.SearchAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateAccountRequest request)
        {
            try
            {
                await _accountService.UpdateAsync(id, request);
                return Ok(new
                {
                    Message = "Cập nhật account thành công"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _accountService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    Error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await _accountService.GetAllAsync(page, pageSize);
            return Ok(result);
        }


    }
}
