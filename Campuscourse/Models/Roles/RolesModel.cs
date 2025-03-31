namespace Campuscourse.Models.Roles;

public class RolesModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserModel UserModel { get; set; }
    public bool IsTeacher { get; set; } = false;
    public bool IsStudent { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
}