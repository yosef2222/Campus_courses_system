using System.ComponentModel.DataAnnotations;

namespace Campuscourse.Models.CampusCourseNotification;

public class CreateNotificationDto
{
    [Required]
    [StringLength(500, ErrorMessage = "Notification text cannot exceed 500 characters.")]
    public string Text { get; set; }

    [Required]
    public bool IsImportant { get; set; }
}