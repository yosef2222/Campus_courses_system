namespace Campuscourse.Models.CampusCourseStudent;

public class CampusCourseStudentResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public string MidtermResult { get; set; }
    public string FinalResult { get; set; }
}
