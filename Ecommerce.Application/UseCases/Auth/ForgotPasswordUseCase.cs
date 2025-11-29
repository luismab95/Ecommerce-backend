

using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Domain.DTOs.Email;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Application.UseCases.Auth;

public class ForgotPasswordUseCase(IAuthService authService, IUserRepository userRepository, IEmailService emailService, IConfiguration config)
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;
    private readonly IEmailService _emailService = emailService;
    private readonly IConfiguration _config = config;


    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {

        var findUser = await _userRepository.GetByEmailAsync(request.Email) ??
            throw new InvalidOperationException("El usuario no esta registrado.");
        
        var  resetPwdAccessToken = await _authService.GenerateResetPasswordTokenAsync(findUser);

        var msg = new EmailMessage
        {
            Body = GetPasswordResetTemplate(),
            From = _config["Gmail:Username"]!,
            IsHtml = true,
            Subject = "Restablecer tu contraseña",
            To = new List<string> { request.Email }
        };

        await _emailService.SendAsync(msg);

        return resetPwdAccessToken;

    }

    public static string GetPasswordResetTemplate()
    {
        return $@" <div style='font-family: Arial, sans-serif; max-width: 480px; margin: auto; padding: 20px;'> <h2 style='color:#333;'>Restablecer tu contraseña</h2> <p style='color:#555; font-size: 15px;'>
                    Has solicitado restablecer tu contraseña. </p>

                    <div style='text-align: center; margin: 30px 0;'>
                        <a 
                           style='background: #4a90e2; color: white; padding: 12px 20px; 
                                  border-radius: 6px; text-decoration: none; font-size: 16px;'>
                            Tienes 10 minutos para realizar el cambio.
                        </a>
                    </div>

                    <p style='color:#777; font-size: 14px;'>
                        Si no solicitaste este cambio, simplemente ignora este mensaje.
                    </p>

                    <p style='color:#aaa; font-size: 12px; margin-top: 20px;'>
                        © {DateTime.UtcNow.Year} Mi Aplicación — Seguridad de cuentas
                    </p>
                </div>";
    }

}
