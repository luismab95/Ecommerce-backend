using System.ComponentModel.DataAnnotations;
using static Ecommerce.Domain.Entities.OrderStatus;

namespace Ecommerce.Application.DTOs.Orders;

public class UpdateOrderRequest
{
    [Required(ErrorMessage = "El Estado es requerido")]
    [EnumDataType(typeof(OrderStatusType), ErrorMessage = "El Estado no es válido")]
    public OrderStatusType Status { get; set; }
}
