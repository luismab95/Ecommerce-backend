using Ecommerce.Api.Filters;
using Ecommerce.Domain.DTOs.General;
using Ecommerce.Domain.DTOs.Products;
using Ecommerce.Application.UseCases.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductUseCases _productUseCases;

    public ProductController(ProductUseCases productUseCases)
    {
        _productUseCases = productUseCases;
    }


    [HttpGet("")]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsWithFiltersRequest request)
    {
        try
        {
            var result = await _productUseCases.GetProductsAsync(request);

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

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        try
        {
            var result = await _productUseCases.GetProductByIdAsync(productId);

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
    public async Task<IActionResult> AddProduct([FromBody] ProductRequest request)
    {
        try
        {
            var result = await _productUseCases.AddProductAsync(request);

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

    [HttpPut("{productId}")]
    [Authorize]
    [ServiceFilter(typeof(PostAuthorizeFilter))]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductRequest request)
    {
        try
        {
            var result = await _productUseCases.UpdateProductAsync(productId, request);

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


    [HttpDelete("{productId}")]
    [Authorize]
    [ServiceFilter(typeof(PostAuthorizeFilter))]
    [ServiceFilter(typeof(PostAuthorizeRoleFilter))]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        try
        {
            var result = await _productUseCases.DeleteProductAsync(productId);

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
