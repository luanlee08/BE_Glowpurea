using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class PublicProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public PublicProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ================= USER LIST =================
        [HttpGet]
        public async Task<IActionResult> GetForUser()
        {
            var products = await _productService.GetForUserAsync();
            return Ok(products);
        }

        // ================= USER DETAIL =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
        public async Task<IActionResult> GetPagedForUser(
           [FromQuery] UserProductPagingRequest request)
        {
            var result =
                await _productService.GetForUserPagedAsync(request);

            return Ok(result);
        }
    }
}
