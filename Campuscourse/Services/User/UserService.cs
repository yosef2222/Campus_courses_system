using Campuscourse.Data;
using Campuscourse.Models.Roles;
using Campuscourse.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Services.User;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(user => new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName
            })
            .ToListAsync();
    }

    public async Task<RolesResponseDto> GetCurrentUserRoles(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || user.Role == null)
        {
            throw new KeyNotFoundException("User or roles not found.");
        }

        return new RolesResponseDto
        {
            IsTeacher = user.Role.IsTeacher,
            IsStudent = user.Role.IsStudent,
            IsAdmin = user.Role.IsAdmin
        };
    }

    public async Task GrantRole(Guid userId, string role)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || user.Role == null)
        {
            throw new KeyNotFoundException("User or roles not found.");
        }

        switch (role.ToLower())
        {
            case "teacher":
                user.Role.IsTeacher = true;
                break;
            case "student":
                user.Role.IsStudent = true;
                break;
            case "admin":
                user.Role.IsAdmin = true;
                break;
            default:
                throw new InvalidOperationException("Invalid role.");
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}