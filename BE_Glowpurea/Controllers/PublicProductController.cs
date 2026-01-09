using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/public/products")]
    public class PublicProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public PublicProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ================= PUBLIC LIST + SEARCH + PAGING =================
        // GET /api/public/products?keyword=son&page=1&pageSize=8
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] UserProductPagingRequest request)
        {
            var result = await _productService.GetForUserPagedAsync(request);
            return Ok(result);
        }

        // ================= PUBLIC DETAIL =================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound(new
                {
                    Message = "Không tìm thấy sản phẩm"
                });

            return Ok(product);
        }
    }
}
