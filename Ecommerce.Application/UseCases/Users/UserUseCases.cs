using Ecommerce.Application.DTOs.General;
using Ecommerce.Application.DTOs.Users;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;

namespace Ecommerce.Application.UseCases.Users;

public class UserUseCases(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<object> GetUserByIdAsync(int userId)
    {
        var findUser = await _userRepository.GetByIdAsync(userId) ??
            throw new InvalidOperationException("Usuario no encontrado.");

        return User.ToSafeResponse(findUser);
    }

    public async Task<object> GetUsersAsync(GeneralPaginationRequest request)
    {
        var result =  await _userRepository.GetUsersAsync(request.PageSize, request.PageNumber,request.SearchTerm);

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

        var updateUser = User.UpdateNames(findUser,user.FirstName, user.LastName);
        await _userRepository.UpdateAsync(updateUser);

        return "Usuario actualizado exitosamente.";

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

        if(findUser.Role != "Cliente")
        {
            throw new InvalidOperationException("No se puede eliminar a un Usuario administrador.");
        }

        var updateUser = User.Delete(findUser);
        await _userRepository.UpdateAsync(updateUser);

        return "Usuario eliminado exitosamente.";
    }

}
