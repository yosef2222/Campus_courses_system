using Campuscourse.Models.CampusGroup;
using Campuscourse.Services.CampusCourse;

namespace Campuscourse.Services.Group;

public interface ICampusGroupService
{
    Task<List<CampusGroupResponseDto>> GetAllGroupsAsync();
    Task<List<SimplifiedCampusCourseDto>> GetCoursesByGroupIdAsync(Guid groupId);
    Task<CampusGroupModel> CreateGroupAsync(string name);
    Task<CampusGroupModel> EditGroupAsync(Guid id, string name);
    Task<List<CampusCourseResponseDto>> DeleteGroupAsync(Guid id);
}