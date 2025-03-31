using System.ComponentModel.DataAnnotations;

namespace Campuscourse.Models.CampusCourseTeacher;

public class AddTeacherToCourseDto
{
    [Required]
    public Guid UserId { get; set; }
}