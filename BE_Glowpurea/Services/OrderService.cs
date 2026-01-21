using BE_Glowpurea.Dtos.Order;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbGlowpureaContext _context;
        private readonly IOrderRepository _orderRepository;


        public OrderService(DbGlowpureaContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }


        public async Task<CreateOrderResponse> CreateOrderAsync(
    int accountId,
    CreateOrderRequest request
)

        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.Cart)
                .Where(c => c.Cart.AccountId == accountId)
                .ToListAsync();


                if (!cartItems.Any())
                    throw new Exception("Giỏ hàng trống");

                var pendingStatusId = await _context.StatusOrders
    .Where(s => s.StatusName == "Pending")
    .Select(s => s.StatusId)
    .FirstOrDefaultAsync();

                if (pendingStatusId == 0)
                    throw new Exception("Status 'Pending' không tồn tại");
                var order = new Order
                {
                    AccountId = accountId,
                    CreatedAt = DateTime.Now,
                    TotalAmount = cartItems.Sum(c => c.PriceAtThatTime * c.Quantity),
                    StatusId = pendingStatusId // ✅ ĐÚNG
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderDetails = cartItems.Select(item => new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.PriceAtThatTime,
                    Discount = 0,
                    Total = item.PriceAtThatTime * item.Quantity,
                    CreatedAt = DateTime.Now,
                    Reviewed = false,
                    IsDeleted = false
                }).ToList();

                _context.OrderDetails.AddRange(orderDetails);


                _context.CartItems.RemoveRange(cartItems);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CreateOrderResponse
                {
                    OrderId = order.OrderId,
                    Message = "Đặt hàng thành công"
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderListResponse>> GetMyOrdersAsync(int accountId)
        {
            return await _context.Orders
                .Where(o => o.AccountId == accountId)
                .Include(o => o.Status) // ✅ QUAN TRỌNG
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderListResponse
                {
                    OrderId = o.OrderId,
                    CreatedAt = o.CreatedAt,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.StatusName
                })
                .ToListAsync();
        }

        public async Task<OrderDetailResponse> GetMyOrderDetailAsync(int accountId, int orderId)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(orderId, accountId);

            if (order == null)
                throw new Exception("Không tìm thấy đơn hàng");

            return new OrderDetailResponse
            {
                OrderId = order.OrderId,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Status = order.Status.StatusName,
                Items = order.OrderDetails
                    .Where(od => !od.IsDeleted)
                    .Select(od => new OrderItemResponse
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    })
                    .ToList()
            };
        }


        //Admin
        public async Task<List<AdminOrderListResponse>> GetAllOrdersForAdminAsync()
        {
            var orders = await _orderRepository.GetAllForAdminAsync();

            return orders.Select(o => new AdminOrderListResponse
            {
                OrderId = o.OrderId,
                AccountId = o.AccountId,
                AccountName = o.Account.AccountName,
                Email = o.Account.Email,
                TotalAmount = o.TotalAmount,
                Status = o.Status.StatusName,
                CreatedAt = o.CreatedAt
            }).ToList();
        }

        //admin
        public async Task<AdminOrderDetailResponse> GetOrderDetailForAdminAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdForAdminAsync(orderId);

            if (order == null)
                throw new Exception("Không tìm thấy đơn hàng");

            return new AdminOrderDetailResponse
            {
                OrderId = order.OrderId,
                CustomerName = order.Account.AccountName,
                Email = order.Account.Email,
                Status = order.Status.StatusName,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,

                Items = order.OrderDetails
                    .Where(od => !od.IsDeleted) // ✅ tránh item đã xoá
                    .Select(od => new AdminOrderItemResponse
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName, // 🔥 LẤY ĐƯỢC
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    })
                    .ToList()
            };
        }

        public async Task UpdateOrderStatusAsync(int orderId, int statusId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("Không tìm thấy đơn hàng");

            var statusExists = await _context.StatusOrders
                .AnyAsync(s => s.StatusId == statusId);

            if (!statusExists)
                throw new Exception("Trạng thái không hợp lệ");

            order.StatusId = statusId;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

    }
}
