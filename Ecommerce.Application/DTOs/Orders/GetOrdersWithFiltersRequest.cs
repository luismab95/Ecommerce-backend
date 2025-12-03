using Ecommerce.Application.DTOs.General;

namespace Ecommerce.Application.DTOs.Orders;

public class GetOrdersWithFiltersRequest : GeneralPaginationRequest
{
    public int? UserId { get; set; }
}
