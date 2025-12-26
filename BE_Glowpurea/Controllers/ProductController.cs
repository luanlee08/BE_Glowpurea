using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            var productId = await _productService.CreateAsync(request);
            return Ok(new
            {
                Message = "Thêm sản phẩm thành công",
                ProductId = productId
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchProductRequest request)
        {
            var result = await _productService.SearchAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
        [FromBody] UpdateProductRequest request)
        {
            try
            {
                await _productService.UpdateAsync(id, request);
                return Ok(new
                {
                    Message = "Cập nhật sản phẩm thành công"
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

    }
}
