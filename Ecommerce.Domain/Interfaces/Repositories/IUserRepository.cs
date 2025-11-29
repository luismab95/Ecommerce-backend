namespace Ecommerce.Domain.Interfaces.Repositories;

using Ecommerce.Domain.DTOs.Pagination;
using Ecommerce.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int userId);
    Task<PaginationResponse<User>> GetUsersAsync(int pageSize, int pageNumber, string? searchTerm);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
}
