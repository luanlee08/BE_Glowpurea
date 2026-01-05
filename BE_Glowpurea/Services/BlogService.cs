using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Blog;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepo;
        private readonly IBlogCategoryRepository _categoryRepo;
        private readonly IWebHostEnvironment _env;

        public BlogService(
            IBlogRepository blogRepo,
            IBlogCategoryRepository categoryRepo,
            IWebHostEnvironment env)
        {
            _blogRepo = blogRepo;
            _categoryRepo = categoryRepo;
            _env = env;
        }

        public async Task<int> CreateAsync(CreateBlogRequest request, int accountId)
        {
            var category = await _categoryRepo.GetByIdsAsync(
                new List<int> { request.BlogCategoryId }
            );

            if (!category.Any())
            {
                throw new Exception("BlogCategory không hợp lệ");
            }

            string? thumbnailUrl = null;
            if (request.BlogThumbnail != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads/blogs");
                Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.BlogThumbnail.FileName)}";
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.BlogThumbnail.CopyToAsync(stream);

                thumbnailUrl = $"/uploads/blogs/{fileName}";
            }

            var blog = new BlogPost
            {
                BlogTitle = request.BlogTitle,
                BlogContent = request.BlogContent,
                BlogThumbnail = thumbnailUrl,
                AccountId = accountId,
                IsPublished = request.IsPublished,
                IsFeatured = request.IsFeatured,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                BlogCategories = category
            };

            await _blogRepo.AddAsync(blog);
            await _blogRepo.SaveChangesAsync();

            return blog.BlogPostId;
        }

        public async Task<PagedResponse<BlogAdminResponse>>
            SearchForAdminAsync(SearchBlogAdminRequest request)
        {
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1) request.PageSize = 10;

            var (blogs, total) = await _blogRepo.SearchForAdminAsync(
                request.Keyword,
                request.Page,
                request.PageSize
            );

            return new PagedResponse<BlogAdminResponse>
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Data = blogs.Select(b => new BlogAdminResponse
                {
                    BlogPostId = b.BlogPostId,
                    BlogTitle = b.BlogTitle,
                    BlogContent = b.BlogContent,
                    BlogThumbnail = b.BlogThumbnail,
                    BlogCategory = b.BlogCategories.FirstOrDefault()?.BlogCategoryName ?? "—",
                    AuthorEmail = b.Account.Email,
                    IsPublished = b.IsPublished,
                    IsFeatured = b.IsFeatured,
                    IsDeleted = b.IsDeleted,
                    CreatedAt = b.CreatedAt
                }).ToList()
            };
        }

        public async Task UpdateAsync(int blogId, UpdateBlogRequest request)
        {
            var blog = await _blogRepo.GetByIdAsync(blogId);

            if (blog == null)
                throw new Exception("Blog không tồn tại");

            var category = await _categoryRepo.GetByIdsAsync(
                new List<int> { request.BlogCategoryId });

            if (!category.Any())
                throw new Exception("BlogCategory không hợp lệ");

            blog.BlogTitle = request.BlogTitle;
            blog.BlogContent = request.BlogContent;
            blog.IsPublished = request.IsPublished;
            blog.IsFeatured = request.IsFeatured;
            blog.IsDeleted = request.IsDeleted;
            blog.BlogCategories = category;

            if (request.BlogThumbnail != null)
            {
                blog.BlogThumbnail = await SaveThumbnailAsync(request.BlogThumbnail);
            }

            await _blogRepo.SaveChangesAsync();
        }

        private async Task<string> SaveThumbnailAsync(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads/blogs");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/blogs/{fileName}";
        }
    }
}