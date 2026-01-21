using BE_Glowpurea.Models;

namespace BE_Glowpurea.IRepositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByAccountIdAsync(int accountId);
        Task<Cart> CreateAsync(Cart cart);

        Task<CartItem?> GetActiveItemAsync(int cartId, int productId);

        Task AddItemAsync(CartItem item);
        Task UpdateItemAsync(CartItem item);
        Task RemoveItemAsync(CartItem item);
        Task RemoveItemsAsync(List<CartItem> items);
        Task<CartItem?> GetItemByIdAsync(int cartItemId);

    }
}
