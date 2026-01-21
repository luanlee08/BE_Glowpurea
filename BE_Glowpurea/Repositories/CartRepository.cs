using BE_Glowpurea.IRepositories;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DbGlowpureaContext _context;

        public CartRepository(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByAccountIdAsync(int accountId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItem?> GetActiveItemAsync(int cartId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.ProductId == productId &&
                ci.Status == "Active");
        }

        public async Task AddItemAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> GetItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        public async Task RemoveItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemsAsync(List<CartItem> items)
        {
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

    }
}
    