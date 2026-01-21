using BE_Glowpurea.Dtos.Order;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int AccountId =>
            int.Parse(User.FindFirst("AccountId")!.Value);

        [HttpPost]
        public async Task<IActionResult> CreateOrder(
    [FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.CreateOrderAsync(AccountId, request);
            return Ok(result);
        }


        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync(AccountId);
            return Ok(orders);
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetMyOrderDetail(int orderId)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")!.Value);
            var result = await _orderService.GetMyOrderDetailAsync(accountId, orderId);
            return Ok(result);
        }

    }
}
