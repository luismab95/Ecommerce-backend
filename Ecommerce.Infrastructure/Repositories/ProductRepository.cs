using Azure.Core;
using Ecommerce.Domain.DTOs.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Image = Ecommerce.Domain.Entities.Image;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);
    }

    public async Task<PaginationResponse<Product>> GetProductsAsync(int pageSize, int pageNumber, string? searchTerm, int? categoryId, decimal? priceMax, decimal? priceMin, bool? featured)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive);

        // Aplicar búsqueda si searchTerm no es nulo o vacío
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();
            query = query.Where(p =>
                 p.Name.ToLower().Contains(searchTerm) ||
                 p.Description.ToLower().Contains(searchTerm)
            );
        }

        // Aplicar filtro por categoría
        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        // Aplicar filtro por precio máximo
        if (priceMax.HasValue && priceMax.Value > 0)
        {
            query = query.Where(p => p.Price <= priceMax.Value);
        }

        // Aplicar filtro por precio mínimo
        if (priceMin.HasValue && priceMin.Value > 0)
        {
            query = query.Where(p => p.Price >= priceMin.Value);
        }

        // Aplicar filtro por productos destacados
        if (featured.HasValue)
        {
            query = query.Where(p => p.Featured == featured.Value);
        }

        query = query.OrderBy(p => p.Id);

       
        return await query.ToPagedListAsync(pageNumber, pageSize);

    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
