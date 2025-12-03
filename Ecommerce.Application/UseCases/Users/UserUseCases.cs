using Ecommerce.Application.DTOs.General;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.DTOs.Users;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Application.UseCases.Users;

public class UserUseCases(IUserRepository userRepository, IProductRepository productRepository, IConfiguration config)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IConfiguration _config = config;

    public async Task<object> GetUserByIdAsync(int userId)
    {
        var findUser = await _userRepository.GetByIdAsync(userId) ??
            throw new InvalidOperationException("Usuario no encontrado.");

        return User.ToSafeResponse(findUser);
    }

    public async Task<object> GetUserWishlistAsync(int userId)
    {
        var result = await _userRepository.GetWishListByUserIdAsync(userId);

        var wishlists = new List<object>();
        result.ForEach(wishlist =>
        {
            if (wishlist != null && wishlist.Product!.Images != null)
            {
                string baseUrl = $"{_config["App:StaticUrl"]}";
                wishlist.Product.Images.ForEach(image =>
                {
                    image = Image.UpdatePath(image, baseUrl);
                });
            }
            wishlists.Add(Product.ToSafeResponse(wishlist!.Product!));
        });



        return wishlists;
    }

    public async Task<string> AddProductWishListAsync(AddProductWishListRequest request, int userId)
    {

        var findProduct = await _productRepository.GetByIdAsync(request.ProductId) ??
            throw new InvalidOperationException("Producto no encontrado.");

        await _userRepository.AddProductWishListAsync(findProduct.Id, userId);

        return "Lista de deseados actualizada exitosamente.";
    }


    public async Task<object> GetUsersAsync(GeneralPaginationRequest request)
    {
        var result = await _userRepository.GetUsersAsync(request.PageSize, request.PageNumber, request.SearchTerm);

        var safeUserResponse = new List<object>();
        result.Items.ForEach(user =>
        {
            safeUserResponse.Add(User.ToSafeResponse(user));
        });

        return new
        {
            Items = safeUserResponse,
            result.TotalCount,
            result.PageNumber,
            result.PageSize,
            result.TotalPages,
            result.HasPreviousPage,
            result.HasNextPage,
        };
    }

    public async Task<string> UpdateUserAsync(int userId, UpdateUserRequest user)
    {
        var findUser = await _userRepository.GetByIdAsync(userId) ??
            throw new InvalidOperationException("Usuario no encontrado.");

        var updateUser = User.Update(findUser, user.FirstName, user.LastName, user.Email,user.Phone);
        await _userRepository.UpdateAsync(updateUser);

        return "Usuario actualizado exitosamente.";

    }


    public async Task<string> UpdateUserAddressAsync(int userId, UpdateUserAddressRequest request)
    {
        var findUser = await _userRepository.GetByIdAsync(userId) ??
            throw new InvalidOperationException("Usuario no encontrado.");

        if (findUser.UserAddress is null)
        {
            var createUser = UserAddress.Create(findUser.Id, request.ShippingAddress.Street, request.ShippingAddress.City, request.ShippingAddress.State, request.ShippingAddress.Code, request.ShippingAddress.Country, request.BillingAddress.Street, request.BillingAddress.City, request.BillingAddress.State, request.BillingAddress.Code, request.BillingAddress.Country);
            await _userRepository.CreateUserAddressAsync(createUser);

        }
        else
        {
            var updateUser = UserAddress.Update(findUser.UserAddress, request.ShippingAddress.Street, request.ShippingAddress.City, request.ShippingAddress.State, request.ShippingAddress.Code, request.ShippingAddress.Country, request.BillingAddress.Street, request.BillingAddress.City, request.BillingAddress.State, request.BillingAddress.Code, request.BillingAddress.Country);
            await _userRepository.UpdateUserAddressAsync(updateUser);
        }


        return "Dirección actualizada exitosamente.";

    }


    public async Task<string> UpdateRoleAsync(int userId)
    {
        var findUser = await _userRepository.GetByIdAsync(userId) ??
            throw new InvalidOperationException("Usuario no encontrado.");

        var updateUser = User.UpdateRole(findUser);
        await _userRepository.UpdateAsync(updateUser);

        return "Rol de usuario actualizado exitosamente.";
    }

    public async Task<string> DeleteUserAsync(int userId)
    {
        var findUser = await _userRepository.GetByIdAsync(userId) ??
            throw new InvalidOperationException("Usuario no encontrado.");

        if (findUser.Role != "Cliente")
        {
            throw new InvalidOperationException("No se puede eliminar a un Usuario administrador.");
        }

        var updateUser = User.Delete(findUser);
        await _userRepository.UpdateAsync(updateUser);

        return "Usuario eliminado exitosamente.";
    }

}
