using System.ComponentModel.DataAnnotations;
using Campuscourse.Models.CampusCourse;
using Campuscourse.Models.Enums;

namespace Campuscourse.Models.CampusCourseStudent;

public class CampusCourseStudentModel
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public CampusCourseModel Course { get; set; }
    public Guid UserId { get; set; } 
    public UserModel User { get; set; }
    public StudentStatuses Status { get; set; }
    public StudentMarks MidtermResult { get; set; }
    public StudentMarks FinalResult { get; set; } 
}
