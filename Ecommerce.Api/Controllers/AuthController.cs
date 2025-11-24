namespace Ecommerce.Api.Controllers;

using Ecommerce.Application.DTOs.General;
using Ecommerce.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = Application.DTOs.Auth.RegisterRequest;

[ApiController]
[Route("api/auth")]
public class AuthController(RegisterUserUseCase registerUserUseCase) : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase = registerUserUseCase;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _registerUserUseCase.RegisterUserAsync(request);

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
