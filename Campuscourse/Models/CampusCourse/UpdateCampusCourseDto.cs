namespace Campuscourse.Models.CampusCourse;

public class UpdateCampusCourseDto
{
    public string Name { get; set; }
    public int StartYear { get; set; }
    public int MaximumStudentsCount { get; set; }
    public string Semester { get; set; }
    public string Requirements { get; set; }
    public string Annotations { get; set; }
    public Guid MainTeacherId { get; set; }
}
