using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.Products;

public class AddProductWishListRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El producto debe tener un ID  válido")]
    public int ProductId { get; set; }
}
