namespace Campuscourse.Models.CampusCourse;

public class CampusCourseResponseAsTeacherDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int StartYear { get; set; }
    public int MaximumStudentsCount { get; set; }
    public int RemainingSlotsCount { get; set; }
    public string Status { get; set; }
    public string Semester { get; set; }
}