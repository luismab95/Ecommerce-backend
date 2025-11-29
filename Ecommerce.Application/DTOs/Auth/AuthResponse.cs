using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public required object User { get; set; }

}

