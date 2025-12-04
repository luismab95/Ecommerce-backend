using System.ComponentModel.DataAnnotations;
using static Ecommerce.Domain.Entities.OrderStatus;

namespace Ecommerce.Application.DTOs.Orders;

public class UpdateOrderRequest
{
    [Required(ErrorMessage = "El Estado es requerido")]
    public string Status { get; set; } = string.Empty;
}
