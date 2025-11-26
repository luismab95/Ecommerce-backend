namespace Ecommerce.Application.UseCases.Auth;

using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;

public class SignInUseCase(IUserRepository userRepository, ISessionRepository sessionRepository, IAuthService authService)
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IAuthService _authService = authService;

    public async Task<AuthResponse> LoginUserAsync(SignInRequest request)
    {
        // Validar que el email exista
        var findUser = await _userRepository.GetByEmailAsync(request.Email) ??
            throw new InvalidOperationException("Creedenciales Incorrectas.");


        // Valid password
        var isValidPassword = _authService.VerifyPassword(request.Password, findUser.PasswordHash);
        if (!isValidPassword)
        {
            throw new InvalidOperationException("Creedenciales Incorrectas.");
        }
              
        // Generar refreshToken
        var refreshToken = await _authService.GenerateRefreshTokenAsync(findUser);

        // Generar session 
        var session = Session.Create(findUser.Id, request.DeviceInfo, request.Ip, refreshToken, true);
        var newSession =  await _sessionRepository.AddAsync(session);

        // Generar accessToken
        var accessToken = await _authService.GenerateTokenAsync(findUser,newSession.Id);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

}
