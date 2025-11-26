namespace Ecommerce.Domain.Interfaces.Repositories;

using Ecommerce.Domain.Entities;

public interface ISessionRepository
{
    Task<Session?> GetSessionAsync(int id);
    Task<Session> AddAsync(Session session);
    Task UpdateAsync(Session session);
    
}
