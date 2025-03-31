using System.ComponentModel.DataAnnotations;

namespace Campuscourse.Models.CampusCourse;

public class UpdateCourseStatusDto
{
    [Required]
    public string Status { get; set; }
}