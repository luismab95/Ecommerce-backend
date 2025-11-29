namespace Ecommerce.Api.Controllers;

using Ecommerce.Api.Filters;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.DTOs.General;
using Ecommerce.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController(SignUpUseCase signUpUseCase, SignInUseCase signInUseCase, SignOutUseCase signOutUseCase, RefreshTokenUseCase refreshTokenUseCase, ForgotPasswordUseCase forgotPasswordUseCase, ResetPasswordUseCase resetPasswordUseCase) : ControllerBase
{
    private readonly SignUpUseCase _signUpUseCase = signUpUseCase;
    private readonly SignInUseCase _signInUseCase = signInUseCase;
    private readonly SignOutUseCase _signOutUseCase = signOutUseCase;
    private readonly ForgotPasswordUseCase _forgotPasswordUseCase = forgotPasswordUseCase;
    private readonly ResetPasswordUseCase _resetPasswordUseCase = resetPasswordUseCase;
    private readonly RefreshTokenUseCase _refreshTokenUseCase = refreshTokenUseCase;


    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        try
        {
            var result = await _signUpUseCase.RegisterUserAsync(request);

            return Ok(new GeneralResponse
            {
                Data = result,
                Message = "Proceso realizado con éxito."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GeneralResponse { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor" });
        }
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        try
        {
            request.Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            request.DeviceInfo = HttpContext.Request.Headers.UserAgent.ToString();

            var result = await _signInUseCase.LoginUserAsync(request);

            return Ok(new GeneralResponse
            {
                Data = result,
                Message = "Proceso realizado con éxito."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GeneralResponse { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor" });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return Unauthorized(new GeneralResponse { Message = "Token no presente en la solicitud." });
            }

            var parts = authHeader.ToString().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !parts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized(new GeneralResponse { Message = "Formato de token inválido." });
            }

            var token = parts[1];
            var result = await _refreshTokenUseCase.RefreshTokenAsync(token);

            return Ok(new GeneralResponse
            {
                Data = result,
                Message = "Proceso realizado con éxito."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GeneralResponse { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor" });
        }
    }


    [HttpPost("sign-out")]
    [Authorize]
    [ServiceFilter(typeof(PostAuthorizeFilter))]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var rawAuth = HttpContext.Request.Headers.Authorization.ToString();
            var token = rawAuth.Replace("Bearer ", "").Trim();

            var result = await _signOutUseCase.LogoutUserAsync(token);

            return Ok(new GeneralResponse
            {
                Data = result,
                Message = "Proceso realizado con éxito."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GeneralResponse { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor" });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            var result = await _forgotPasswordUseCase.ForgotPasswordAsync(request);

            return Ok(new GeneralResponse
            {
                Data = result,
                Message = "Proceso realizado con éxito."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GeneralResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor" + ex.ToString() });
        }
    }

    [HttpPost("reset-password")]
    [Authorize]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var rawAuth = HttpContext.Request.Headers.Authorization.ToString();
            var token = rawAuth.Replace("Bearer ", "").Trim();
            request.Token = token;


            var result = await _resetPasswordUseCase.ResetPasswordAsync(request);

            return Ok(new GeneralResponse
            {
                Data = result,
                Message = "Proceso realizado con éxito."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GeneralResponse { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor" });
        }
    }

}
