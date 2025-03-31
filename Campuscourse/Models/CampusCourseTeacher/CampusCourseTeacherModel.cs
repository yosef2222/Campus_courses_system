using System.ComponentModel.DataAnnotations;
using Campuscourse.Models.CampusCourse;

namespace Campuscourse.Models.CampusCourseTeacher;

public class CampusCourseTeacherModel
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public CampusCourseModel Course { get; set; }
    public Guid UserId { get; set; }
    public UserModel User { get; set; }
    public bool IsMain { get; set; }
}
