using Ecommerce.Api.Filters;
using Ecommerce.Application.DTOs.General;
using Ecommerce.Application.DTOs.Users;
using Ecommerce.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;


[ApiController]
[Authorize]
[ServiceFilter(typeof(PostAuthorizeFilter))]
[Route("api/users")]
public class UserController(UserUseCases userUseCase) : ControllerBase
{
    private readonly UserUseCases _userUseCase = userUseCase;


    [HttpGet("")]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> GetUsers([FromQuery] GeneralPaginationRequest request)
    {
        try
        {
            var result = await _userUseCase.GetUsersAsync(request);

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
            return StatusCode(500, new GeneralResponse { Message = "Error interno del servidor " });
        }
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var result = await _userUseCase.GetUserByIdAsync(userId);

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


    [HttpPut("profile/{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var result = await _userUseCase.UpdateUserAsync(userId, request);

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

    [HttpPut("role/{userId}")]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> UpdateUserRole(int userId)
    {
        try
        {
            var result = await _userUseCase.UpdateRoleAsync(userId);

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


    [HttpDelete("{userId}")]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            var result = await _userUseCase.DeleteUserAsync(userId);

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
