using Campuscourse.Models;
using Campuscourse.Models.Users;

namespace Campuscourse.Services.Auth;

public interface IAuthService
{
    Task<string> Register(UserDto request);
    Task<string> Login(LoginDto loginDto);
    Task Logout(string token, string expirationClaim);
    Task<UserProfileDto> GetProfile(Guid userId);
    Task<UserProfileDto> EditProfile(Guid userId, EditProfileDto request);
}