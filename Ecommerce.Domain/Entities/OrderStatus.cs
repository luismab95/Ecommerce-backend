using System.Security.Cryptography;

namespace Ecommerce.Domain.Entities;

public class OrderStatus
{

    public enum OrderStatusType
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public OrderStatusType Status { get; private set; } = OrderStatusType.Pending;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    public virtual Order? Order { get; set; }


    private OrderStatus() { }

    public static OrderStatus Cancel(OrderStatus orderStatus)
    {
        orderStatus.Status = OrderStatusType.Cancelled;
        orderStatus.UpdatedAt = DateTime.UtcNow;
        return orderStatus;
    }

    public static OrderStatus SetStatus(OrderStatus orderStatus, OrderStatusType status)
    {
        orderStatus.Status = status;
        orderStatus.UpdatedAt = DateTime.UtcNow;
        return orderStatus;
    }

    public static OrderStatus Create()
    {
        return new OrderStatus
        {
            Status = OrderStatusType.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }


}