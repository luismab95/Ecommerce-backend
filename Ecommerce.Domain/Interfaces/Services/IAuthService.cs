using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<string> GenerateTokenAsync(User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
