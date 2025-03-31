using System.ComponentModel.DataAnnotations;

namespace Campuscourse.Models.Users;

public class EditProfileDto
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
}
