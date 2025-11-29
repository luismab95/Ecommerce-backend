namespace Ecommerce.Api.Controllers;

using Ecommerce.Api.Filters;
using Ecommerce.Application.DTOs.Categories;
using Ecommerce.Application.DTOs.General;
using Ecommerce.Application.UseCases.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/categories")]
public class CategoryController(CategoryUseCases categoryUseCases) : ControllerBase
{
    private readonly CategoryUseCases _categoryUseCases = categoryUseCases;

    [HttpGet("")]
    public async Task<IActionResult> GetCategories([FromQuery] GeneralPaginationRequest request)
    {
        try
        {
            var result = await _categoryUseCases.GetCategoriesAsync(request);

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

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryById(int categoryId)
    {
        try
        {
            var result = await _categoryUseCases.GetCategoryByIdAsync(categoryId);

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

    [HttpPost("")]
    [Authorize]
    [ServiceFilter(typeof(PostAuthorizeFilter))]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
    {
        try
        {
            var result = await _categoryUseCases.AddCategoryAsync(request);

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



    [HttpPut("{categoryId}")]
    [Authorize]
    [ServiceFilter(typeof(PostAuthorizeFilter))]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryRequest request)
    {
        try
        {
            var result = await _categoryUseCases.UpdateCategoryAsync(categoryId, request);

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


    [HttpDelete("{categoryId}")]
    [Authorize]
    [ServiceFilter(typeof(PostAuthorizeFilter))]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            var result = await _categoryUseCases.DeleteCategoryAsync(categoryId);

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

