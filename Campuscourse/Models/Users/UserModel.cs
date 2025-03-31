using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.Roles;
namespace Campuscourse.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime Birthday { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public RolesModel Role { get; set; }
    public ICollection<CampusCourseTeacherModel> TeachingCourses { get; set; }

}