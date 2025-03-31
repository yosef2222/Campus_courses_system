using System.ComponentModel.DataAnnotations;
using Campuscourse.Models.CampusCourse;

namespace Campuscourse.Models.CampusCourseNotification;

public class CampusCourseNotificationModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CourseId { get; set; }
    public CampusCourseModel Course { get; set; }
    public string Text { get; set; }
    public bool IsImportant { get; set; }
}