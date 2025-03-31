using System.ComponentModel.DataAnnotations;
using Campuscourse.Models;
using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.CampusGroup;
using Campuscourse.Models.Enums;

namespace Campuscourse.Models.CampusCourse;

public class CampusCourseModel
{
    public Guid Id { get; set; } 
    public string Name { get; set; } 
    public int StartYear { get; set; }
    public int MaximumStudentsCount { get; set; }
    public string Requirements { get; set; } 
    public string Annotations { get; set; }
    public CourseStatuses Status { get; set; }
    public Semesters Semester { get; set; } 
    public Guid CampusGroupId { get; set; }
    public Guid MainTeacherId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int RemainingSlotsCount { get; set; }
    public CampusGroupModel CampusGroup { get; set; }
    public ICollection<CampusCourseStudentModel> Students { get; set; }
    public ICollection<CampusCourseTeacherModel> Teachers { get; set; }
    public ICollection<CampusCourseNotificationModel> Notifications { get; set; }
}
