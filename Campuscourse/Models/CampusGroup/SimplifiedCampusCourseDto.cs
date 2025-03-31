namespace Campuscourse.Models.CampusGroup;

public class SimplifiedCampusCourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int StartYear { get; set; }
    public int MaximumStudentsCount { get; set; }
    public int RemainingSlotsCount { get; set; }
    public string Status { get; set; }
    public string Semester { get; set; }
}