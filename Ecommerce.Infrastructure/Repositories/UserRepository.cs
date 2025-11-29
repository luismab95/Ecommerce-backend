namespace Ecommerce.Infrastructure.Repositories;

using Ecommerce.Domain.DTOs.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{

    private readonly ApplicationDbContext _context = context;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<PaginationResponse<User>> GetUsersAsync(int pageSize, int pageNumber, string? searchTerm)
    {
        var query = _context.Users.Where(u => u.IsActive);

        // Aplicar búsqueda si searchTerm no es nulo o vacío
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();
            query = query.Where(u =>
                u.FirstName.ToLower().Contains(searchTerm) ||
                u.LastName.ToLower().Contains(searchTerm) ||
                u.Email.ToLower().Contains(searchTerm)
            );
        }

        query = query.OrderBy(u => u.Id);

        return await query.ToPagedListAsync(pageNumber, pageSize);
    }

  
    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && u.IsActive);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
