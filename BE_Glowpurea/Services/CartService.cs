using BE_Glowpurea.Dtos.Cart;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly DbGlowpureaContext _context;
        public CartService(
            ICartRepository cartRepo,
            IProductRepository productRepo,
            DbGlowpureaContext context)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _context = context;
        }

        public async Task AddToCartAsync(int accountId, AddToCartRequest request)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Số lượng không hợp lệ");

            var product = await _productRepo.GetByIdAsync(request.ProductId)
                ?? throw new ArgumentException("Sản phẩm không tồn tại");

            if (product.ProductStatus != "Available" || product.Quantity <= 0)
                throw new ArgumentException("Sản phẩm không thể bán");

            // 1️⃣ Lấy cart
            var cart = await _cartRepo.GetCartByAccountIdAsync(accountId);

            if (cart == null)
            {
                cart = await _cartRepo.CreateAsync(new Cart
                {
                    AccountId = accountId,
                    CreatedAt = DateTime.Now
                });
            }

            // 2️⃣ Lấy CartItem Active
            var item = await _cartRepo.GetActiveItemAsync(cart.CartId, product.ProductId);

            if (item != null)
            {
                item.Quantity += request.Quantity;
                await _cartRepo.UpdateItemAsync(item);
            }
            else
            {
                await _cartRepo.AddItemAsync(new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Quantity = request.Quantity,
                    PriceAtThatTime = product.Price,
                    Status = "Active",
                    AddedAt = DateTime.Now
                });
            }

            cart.UpdatedAt = DateTime.Now;
        }

        public async Task<List<CartItemDto>> GetCartAsync(int accountId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (cart == null)
                return new List<CartItemDto>();

            return cart.CartItems
                .Where(ci => ci.Status == "Active")
                .Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,

                    // ✅ LẤY ẢNH CHÍNH
                    ImageUrl = ci.Product.ProductImages
                        .FirstOrDefault(img => img.IsMain)?.ImageUrl ?? "",

                    Price = ci.PriceAtThatTime,
                    Quantity = ci.Quantity
                })
                .ToList();
        }

        public async Task RemoveItemAsync(int accountId, int cartItemId)
        {
            var item = await _cartRepo.GetItemByIdAsync(cartItemId);

            if (item == null)
                throw new Exception("Sản phẩm không tồn tại trong giỏ");

            if (item.Cart.AccountId != accountId)
                throw new UnauthorizedAccessException("Không có quyền");

            await _cartRepo.RemoveItemAsync(item);
        }

        public async Task ClearCartAsync(int accountId)
        {
            var cart = await _cartRepo.GetCartByAccountIdAsync(accountId);

            if (cart == null || !cart.CartItems.Any())
                return;

            var activeItems = cart.CartItems
                .Where(ci => ci.Status == "Active")
                .ToList();

            await _cartRepo.RemoveItemsAsync(activeItems);

            cart.UpdatedAt = DateTime.Now;
        }
        public async Task UpdateItemQuantityAsync(
            int accountId,
            int cartItemId,
            int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Số lượng không hợp lệ");

            var item = await _cartRepo.GetItemByIdAsync(cartItemId)
                ?? throw new ArgumentException("Cart item không tồn tại");

            // 🔐 Check quyền
            if (item.Cart.AccountId != accountId)
                throw new UnauthorizedAccessException("Không có quyền cập nhật");

            item.Quantity = quantity;
            item.Cart.UpdatedAt = DateTime.Now;

            await _cartRepo.UpdateItemAsync(item);
        }

    }
}
