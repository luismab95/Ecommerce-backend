using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Ecommerce.Application.UseCases.Auth;

public class RefreshTokenUseCase(ISessionRepository sessionRepository, IAuthService authService, IUserRepository userRepository)
{

    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;

    public async Task<string> RefreshTokenAsync(string token)
    {
        // Obtener payload del token
        var isValidToken = _authService.ValidateToken(token, false);
        if (!isValidToken)
        {
            throw new InvalidOperationException("Token no válido.");
        }
        var jwtPayload = await _authService.GetPayloadJwtTokenAsync(token);

        // Buscar sesión 
        int sessionId = int.Parse(jwtPayload[ClaimTypes.NameIdentifier].ToString()!);
        var session = await _sessionRepository.GetSessionAsync(sessionId) ??
            throw new InvalidOperationException("Sesión no encontrada.");

        // Validar token
        var isValidRefreshToken = _authService.ValidateToken(session.RefreshToken, true);
        if(!isValidRefreshToken)
        {
            throw new InvalidOperationException("Sesión expirada.");
        }

        // Buscar usuario
        var findUser = await _userRepository.GetByEmailAsync(jwtPayload[ClaimTypes.Email].ToString()!) ?? throw new InvalidOperationException("Sesión expirada.");
        
        // Generar nuevo token
        var accessToken = await _authService.GenerateTokenAsync(findUser,sessionId);

        return accessToken;
    }
}
