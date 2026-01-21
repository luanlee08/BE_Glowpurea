using BE_Glowpurea.Dtos.Cart;

namespace BE_Glowpurea.IServices
{
    public interface ICartService
    {
        Task AddToCartAsync(int accountId, AddToCartRequest request);
        Task<List<CartItemDto>> GetCartAsync(int accountId);
        Task RemoveItemAsync(int accountId, int cartItemId);
        Task ClearCartAsync(int accountId);
        Task UpdateItemQuantityAsync(int accountId, int cartItemId, int quantity);

    }
}
