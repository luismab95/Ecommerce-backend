using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Ecommerce.Application.UseCases.Auth
{
    public class SignOutUseCase(ISessionRepository sessionRepository, IAuthService authService)
    {
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly IAuthService _authService = authService;

        public async Task<string> LogoutUserAsync(string token)
        {
            // Obtener payload del token
            var jwtPayload = await _authService.GetPayloadJwtTokenAsync(token);

            // Buscar session 
            int sessionId = int.Parse(jwtPayload[ClaimTypes.NameIdentifier].ToString()!);
            var session = await _sessionRepository.GetSessionAsync(sessionId) ??
                throw new InvalidOperationException("Sesión no encontrada.");


            var updatedSession = Session.Update(session);
            await _sessionRepository.UpdateAsync(updatedSession);

            return "La sesión se ha cerrado correctamente.";
        }
    }
}
