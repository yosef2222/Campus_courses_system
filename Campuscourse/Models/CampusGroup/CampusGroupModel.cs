using System.ComponentModel.DataAnnotations;
using Campuscourse.Models.CampusCourse;

namespace Campuscourse.Models.CampusGroup;

public class CampusGroupModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<CampusCourseModel> Courses { get; set; }
}
