using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/products")]
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ================= CREATE =================
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var productId = await _productService.CreateAsync(request);
                return Ok(new
                {
                    Message = "Thêm sản phẩm thành công",
                    ProductId = productId
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


        // ================= VIEW + SEARCH + PAGINATION =================
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] SearchProductRequest request)
        {
            var result = await _productService.GetPagedAsync(request);
            return Ok(result);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
     int id,
     [FromForm] UpdateProductRequest request)
        {
            try
            {
                await _productService.UpdateAsync(id, request);
                return Ok(new { Message = "Cập nhật sản phẩm thành công" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
