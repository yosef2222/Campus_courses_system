using System.ComponentModel.DataAnnotations;

namespace Campuscourse.Models.CampusCourseStudent;

public class UpdateStudentMarkDto
{
    [Required]
    public string MarkType { get; set; }

    [Required]
    public string Mark { get; set; }
}