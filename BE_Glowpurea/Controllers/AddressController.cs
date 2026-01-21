using BE_Glowpurea.Dtos.Address;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        private int AccountId =>
            int.Parse(User.FindFirst("AccountId")!.Value);

        [HttpGet]
        public async Task<IActionResult> GetMyAddresses()
        {
            var result = await _addressService.GetMyAddressesAsync(AccountId);
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateAddressRequest request)
        {
            var id = await _addressService.CreateAsync(AccountId, request);
            return Ok(new { message = "Thêm địa chỉ thành công", addressId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAddressRequest request)
        {
            await _addressService.UpdateAsync(id, AccountId, request);
            return Ok(new { message = "Cập nhật địa chỉ thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _addressService.DeleteAsync(id, AccountId);
            return Ok(new { message = "Xóa địa chỉ thành công" });
        }

        [HttpPatch("{id}/default")]
        public async Task<IActionResult> SetDefault(int id)
        {
            await _addressService.SetDefaultAsync(id, AccountId);
            return Ok(new { message = "Đặt địa chỉ mặc định thành công" });
        }
    }
}
