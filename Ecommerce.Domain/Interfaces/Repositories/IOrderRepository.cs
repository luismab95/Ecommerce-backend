using Ecommerce.Domain.DTOs.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int orderId);
    Task<PaginationResponse<Order>> GetOrderAsync(int pageSize, int pageNumber, string? searchTerm, int? userId);
    Task AddOrderWithTransactionAsync(Order order);
    Task UpdateAsync(OrderStatus orderStatus);
    Task CancelAsync(Order order, List<OrderItem> orderItems);
}
