using BE_Glowpurea.Dtos.Cart;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var accountId = int.Parse(
                User.FindFirst("AccountId")!.Value);

            await _cartService.AddToCartAsync(accountId, request);

            return Ok(new { Message = "Đã thêm vào giỏ hàng" });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var accountId = int.Parse(User.FindFirst("AccountId")!.Value);

            var cart = await _cartService.GetCartAsync(accountId);

            return Ok(cart);
        }

        // ================= DELETE 1 ITEM =================
        [HttpDelete("items/{cartItemId:int}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")!.Value);

            await _cartService.RemoveItemAsync(accountId, cartItemId);

            return Ok(new { Message = "Đã xóa sản phẩm khỏi giỏ hàng" });
        }

        // ================= CLEAR CART =================
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var accountId = int.Parse(User.FindFirst("AccountId")!.Value);

            await _cartService.ClearCartAsync(accountId);

            return Ok(new { Message = "Đã xóa toàn bộ giỏ hàng" });
        }

        [HttpPatch("items/{cartItemId}")]
        public async Task<IActionResult> UpdateItemQuantity(
            int cartItemId,
            UpdateCartItemRequest request)
        {
            var accountId = int.Parse(
                User.FindFirst("AccountId")!.Value);

            await _cartService.UpdateItemQuantityAsync(
                accountId,
                cartItemId,
                request.Quantity);

            return Ok(new { Message = "Cập nhật số lượng thành công" });
        }

    }
}
