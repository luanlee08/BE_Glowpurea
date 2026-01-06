using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Blog;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/blogs")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicBlogs(
            [FromQuery] string? keyword,
            [FromQuery] int? categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _blogService.GetPublicAsync(
                keyword,
                categoryId,
                page,
                pageSize
            );

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBlogDetail(int id)
        {
            try
            {
                var blog = await _blogService.GetPublicDetailAsync(id);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
        }

        
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentBlogs(
            [FromQuery] int limit = 5)
        {
            var blogs = await _blogService.GetRecentAsync(limit);
            return Ok(blogs);
        }
    }
}
