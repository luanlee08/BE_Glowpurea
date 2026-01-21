using BE_Glowpurea.Dtos.Order;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/admin/orders")]
    public class AdminOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ===== VIEW ALL ORDERS =====
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersForAdminAsync();
            return Ok(result);
        }

        // ===== VIEW ORDER DETAIL =====
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var result = await _orderService.GetOrderDetailForAdminAsync(id);
            return Ok(result);
        }


        // ===== UPDATE STATUS ONLY =====
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateOrderStatusRequest request)
        {
            await _orderService.UpdateOrderStatusAsync(id, request.StatusId);
            return Ok(new { Message = "Cập nhật trạng thái đơn hàng thành công" });
        }
    }
}
