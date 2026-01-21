using BE_Glowpurea.Dtos;
using BE_Glowpurea.Dtos.Order;
using BE_Glowpurea.Models;

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
        Task<PagedResponse<AdminOrderListResponse>> GetPagedOrdersForAdminAsync(int page, int pageSize);
    }

}
