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

        /* ===================== ADMIN ===================== */

        public async Task<int> CreateAsync(CreateBlogRequest request, int accountId)
        {
            var categories = await _categoryRepo.GetByIdsAsync(
                new List<int> { request.BlogCategoryId });

            if (!categories.Any())
                throw new Exception("BlogCategory không hợp lệ");

            string? thumbnailUrl = null;

            if (request.BlogThumbnail != null)
                thumbnailUrl = await SaveThumbnailAsync(request.BlogThumbnail);

            var blog = new BlogPost
            {
                BlogTitle = request.BlogTitle,
                BlogContent = request.BlogContent,
                BlogThumbnail = thumbnailUrl,
                AccountId = accountId,
                IsPublished = request.IsPublished,
                IsFeatured = request.IsFeatured,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                BlogCategories = categories
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
                    CategoryId = b.BlogCategories.FirstOrDefault()?.BlogCategoryId ?? 0,
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

            var categories = await _categoryRepo.GetByIdsAsync(
                new List<int> { request.BlogCategoryId });

            if (!categories.Any())
                throw new Exception("BlogCategory không hợp lệ");

            blog.BlogTitle = request.BlogTitle;
            blog.BlogContent = request.BlogContent;
            blog.IsPublished = request.IsPublished;
            blog.IsFeatured = request.IsFeatured;
            blog.IsDeleted = request.IsDeleted;
            blog.BlogCategories = categories;

            if (request.BlogThumbnail != null)
                blog.BlogThumbnail = await SaveThumbnailAsync(request.BlogThumbnail);

            await _blogRepo.SaveChangesAsync();
        }

        /* ===================== PUBLIC ===================== */

        public async Task<PagedResponse<BlogPublicResponse>> GetPublicAsync(
            string? keyword,
            int? categoryId,
            int page,
            int pageSize)
        {
            var (blogs, total) = await _blogRepo.GetPublicAsync(
                keyword, categoryId, page, pageSize);

            return new PagedResponse<BlogPublicResponse>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Data = blogs.Select(b => new BlogPublicResponse
                {
                    BlogPostId = b.BlogPostId,
                    BlogTitle = b.BlogTitle,
                    BlogExcerpt = b.BlogContent.Length > 150
                        ? b.BlogContent.Substring(0, 150) + "..."
                        : b.BlogContent,
                    BlogThumbnail = b.BlogThumbnail,
                    BlogCategory = b.BlogCategories.First().BlogCategoryName,
                    AuthorEmail = b.Account.Email,
                    CreatedAt = b.CreatedAt
                }).ToList()
            };
        }

        public async Task<BlogDetailResponse> GetPublicDetailAsync(int blogId)
        {
            var blog = await _blogRepo.GetPublicDetailAsync(blogId);

            if (blog == null)
                throw new Exception("Blog không tồn tại");

            return new BlogDetailResponse
            {
                BlogPostId = blog.BlogPostId,
                BlogTitle = blog.BlogTitle,
                BlogContent = blog.BlogContent,
                BlogThumbnail = blog.BlogThumbnail,
                BlogCategory = blog.BlogCategories.First().BlogCategoryName,
                AuthorEmail = blog.Account.Email,
                CreatedAt = blog.CreatedAt
            };
        }

        public async Task<List<BlogPublicResponse>> GetRecentAsync(int limit)
        {
            var blogs = await _blogRepo.GetRecentAsync(limit);

            return blogs.Select(b => new BlogPublicResponse
            {
                BlogPostId = b.BlogPostId,
                BlogTitle = b.BlogTitle,
                BlogExcerpt = b.BlogContent.Length > 120
                    ? b.BlogContent.Substring(0, 120) + "..."
                    : b.BlogContent,
                BlogThumbnail = b.BlogThumbnail,
                BlogCategory = b.BlogCategories.First().BlogCategoryName,
                AuthorEmail = b.Account.Email,
                CreatedAt = b.CreatedAt
            }).ToList();
        }

        /* ===================== HELPER ===================== */

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
