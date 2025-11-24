namespace Ecommerce.Application.UseCases.Auth;

using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using Ecommerce.Application.DTOs.Auth;

public class RegisterUserUseCase(IUserRepository userRepository, IAuthService authService)
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;

    public async Task<String> RegisterUserAsync(RegisterRequest request)
    {
        // Validar que el email no exista
        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Hashear password
        var passwordHash = _authService.HashPassword(request.Password);

        // Crear usuario
        var user = User.Create(request.Email, passwordHash, request.FirstName, request.LastName);

        // Guardar usuario
        await _userRepository.AddAsync(user);

        return "¡Bienvenido! Tu cuenta ha sido creada correctamente. Ya puedes iniciar sesión.";
    }

}
