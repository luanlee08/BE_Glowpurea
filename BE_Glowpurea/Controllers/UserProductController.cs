using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class UserProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public UserProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ✅ GET: /api/products/user?page=1&pageSize=8
        [HttpGet("user")]
        public async Task<IActionResult> GetForUserPaged(
            [FromQuery] UserProductPagingRequest request)
        {
            var result = await _productService.GetForUserPagedAsync(request);
            return Ok(result);
        }

    }
}
