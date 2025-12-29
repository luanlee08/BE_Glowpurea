using BE_Glowpurea.Dtos.Product;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbGlowpureaContext _context;

        public ProductRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<(List<ProductListResponse> Data, int Total)>
            SearchByNameOrSkuAsync(SearchProductRequest request)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Shapes)
                .Include(p => p.ProductImages)
                .Where(p => !p.IsDeleted);


            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();

                query = query.Where(p =>
                    p.ProductName.Contains(keyword) ||
                    p.Sku.Contains(keyword));
            }

            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
             .Select(p => new ProductListResponse
             {
                 ProductId = p.ProductId,
                 Sku = p.Sku,
                 ProductName = p.ProductName,
                 CategoryName = p.Category != null ? p.Category.CategoryName : null,
                 ShapesName = p.Shapes != null ? p.Shapes.ShapesName : null,
                 Price = p.Price,
                 Quantity = p.Quantity,
                 ProductStatus = p.ProductStatus,
                 MainImageUrl = p.ProductImages
        .Where(i => i.IsMain)
        .Select(i => i.ImageUrl)
        .FirstOrDefault()
             })


                .ToListAsync();

            return (data, total);
        }
        public async Task<bool> ExistsByNameAsync(string productName)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductName == productName && !p.IsDeleted);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> ExistsByNameExceptIdAsync(string productName, int productId)
        {
            return await _context.Products.AnyAsync(p =>
                p.ProductName == productName &&
                p.ProductId != productId &&
                !p.IsDeleted);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Product>> GetAllAsync()
        {
                    return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Shapes)
            .Include(p => p.ProductImages) // 👈 thêm
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        }
        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

    }
}
