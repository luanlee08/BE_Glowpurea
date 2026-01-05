using BE_Glowpurea.Dtos.Blog;
using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers
{
    [ApiController]
    [Route("api/admin/blogs")]
    public class AdminBlogController : Controller
    {
        private readonly IBlogService _blogService;

        public AdminBlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] SearchBlogAdminRequest request)
        {
            return Ok(await _blogService.SearchForAdminAsync(request));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateBlogRequest request)

        {
            // TODO: sau này lấy từ JWT
            int accountId = 1;

            var blogId = await _blogService.CreateAsync(request, accountId);

            if (request.BlogThumbnail == null || request.BlogThumbnail.Length == 0)
            {
                return BadRequest(new
                {
                    BlogThumbnail = "Ảnh đại diện là bắt buộc"
                });
            }

            return Ok(new
            {
                message = "Tạo blog thành công",
                blogId
            });
        }


        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
    int id,
    [FromForm] UpdateBlogRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _blogService.UpdateAsync(id, request);
                return Ok(new
                {
                    message = "Cập nhật blog thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

    }
}