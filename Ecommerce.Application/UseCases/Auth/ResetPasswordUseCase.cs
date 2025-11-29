

using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using Ecommerce.Domain.Entities;
using System.Security.Claims;

namespace Ecommerce.Application.UseCases.Auth;

public class ResetPasswordUseCase(IAuthService authService, IUserRepository userRepository)
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;


    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        // Validar token
        var isValidToken = _authService.ValidateToken(request.Token, true);
        if (!isValidToken)
        {
            throw new InvalidOperationException("Token no válido.");
        }

        // Validar email y role
        var jwtPayload = await _authService.GetPayloadJwtTokenAsync(request.Token);
        if (jwtPayload[ClaimTypes.Email].ToString() != request.Email)
        {
            throw new InvalidOperationException("El email proporcionado no coincide con el email de la solicitud de restablecimiento de contraseña.");
        }

        if (jwtPayload[ClaimTypes.Role].ToString() != "RESET_PASSWORD")
        {
            throw new InvalidOperationException("Token no válido.");
        }

        // Buscar usuario
        var findUser = await _userRepository.GetByEmailAsync(request.Email) ??
            throw new InvalidOperationException("El usuario no esta registrado.");

        // actualizar usuario
        var hashPassword = _authService.HashPassword(request.Password);
        var user = User.ResetPassword(findUser, hashPassword);
   
        await _userRepository.UpdateAsync(user);

        return "Tu contraseña ha sido restablecida con éxito.";
    }
}
