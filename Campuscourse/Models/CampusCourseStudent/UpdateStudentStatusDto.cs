using System.ComponentModel.DataAnnotations;

namespace Campuscourse.Models.CampusCourseStudent;

public class UpdateStudentStatusDto
{
    [Required]
    public string Status { get; set; }
}