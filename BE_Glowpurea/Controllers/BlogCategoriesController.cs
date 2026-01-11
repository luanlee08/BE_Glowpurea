using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/blog-categories")]
    public class BlogCategoriesController : ControllerBase
    {
        private readonly IBlogCategoryService _service;

        public BlogCategoriesController(IBlogCategoryService service)
        {
            _service = service;
        }

        // GET: /api/blog-categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
