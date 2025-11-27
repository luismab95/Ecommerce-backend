using Ecommerce.Domain.DTOs.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId);
    Task<PaginationResponse<Product>> GetProductsAsync(int pageSize, int pageNumber, string? searchTerm, int? categoryId, decimal? priceMax, decimal? priceMin, bool? featured);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
}
