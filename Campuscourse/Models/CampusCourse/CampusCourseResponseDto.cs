using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;

namespace Campuscourse.Services.CampusCourse;

public class CampusCourseResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int StartYear { get; set; } 
    public int MaximumStudentsCount { get; set; }
    public int StudentsEnrolledCount { get; set; }
    public int StudentsInQueueCount { get; set; }
    public string Requirements { get; set; }
    public int RemainingSlotsCount { get; set; }
    public string Annotations { get; set; } 
    public string Status { get; set; } 
    public string Semester { get; set; } 
    public List<CampusCourseStudentResponseDto> Students { get; set; } 
    public List<CampusCourseTeacherResponseDto> Teachers { get; set; } 
    public List<CampusCourseNotificationResponseDto> Notifications { get; set; } 
}

