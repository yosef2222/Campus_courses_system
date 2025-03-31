using Campuscourse.Models.Roles;
using Campuscourse.Models.Users;

namespace Campuscourse.Services.User;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<RolesResponseDto> GetCurrentUserRoles(Guid userId);
    Task GrantRole(Guid userId, string role);

}