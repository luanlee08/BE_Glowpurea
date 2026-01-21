using BE_Glowpurea.Dtos.Order;

namespace BE_Glowpurea.IServices
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrderAsync(
            int accountId,
            CreateOrderRequest request
        );

        Task<List<OrderListResponse>> GetMyOrdersAsync(int accountId);
        Task<List<AdminOrderListResponse>> GetAllOrdersForAdminAsync();
        Task<AdminOrderDetailResponse> GetOrderDetailForAdminAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, int statusId);
        Task<OrderDetailResponse> GetMyOrderDetailAsync(int accountId, int orderId);
    }

}
