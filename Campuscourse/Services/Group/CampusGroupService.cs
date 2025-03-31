using Campuscourse.Data;
using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.CampusGroup;
using Campuscourse.Models.Enums;
using Campuscourse.Services.CampusCourse;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Services.Group;

public class CampusGroupService : ICampusGroupService
{
    private readonly AppDbContext _context;

    public CampusGroupService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CampusGroupResponseDto>> GetAllGroupsAsync()
    {
        return await _context.CampusGroups
            .Select(group => new CampusGroupResponseDto
            {
                Id = group.Id,
                Name = group.Name
            })
            .ToListAsync();
    }

    public async Task<List<SimplifiedCampusCourseDto>> GetCoursesByGroupIdAsync(Guid groupId)
    {
        var courses = await _context.CampusGroups
            .Where(g => g.Id == groupId)
            .Include(g => g.Courses)
            .SelectMany(g => g.Courses)
            .Select(c => new SimplifiedCampusCourseDto
            {
                Id = c.Id,
                Name = c.Name,
                StartYear = c.StartYear,
                MaximumStudentsCount = c.MaximumStudentsCount,
                RemainingSlotsCount =
                    c.MaximumStudentsCount - c.Students.Count(s => s.Status == StudentStatuses.Accepted),
                Status = c.Status.ToString(),
                Semester = c.Semester.ToString()
            })
            .ToListAsync();

        if (!courses.Any())
            throw new KeyNotFoundException("No courses found for the specified group.");

        return courses;
    }


    public async Task<CampusGroupModel> CreateGroupAsync(string name)
    {
        var group = new CampusGroupModel { Name = name };
        _context.CampusGroups.Add(group);
        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<CampusGroupModel> EditGroupAsync(Guid id, string name)
    {
        var group = await _context.CampusGroups.FindAsync(id);
        if (group == null)
            throw new KeyNotFoundException("Group not found.");

        group.Name = name;
        _context.CampusGroups.Update(group);
        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<List<CampusCourseResponseDto>> DeleteGroupAsync(Guid id)
    {
        var group = await _context.CampusGroups
            .Include(g => g.Courses)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
            throw new KeyNotFoundException("Group not found.");

        var courses = group.Courses.Select(c => new CampusCourseResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            StartYear = c.StartYear,
            MaximumStudentsCount = c.MaximumStudentsCount,
            RemainingSlotsCount = c.MaximumStudentsCount - c.Students.Count,
            Status = c.Status.ToString(),
            Semester = c.Semester.ToString()
        }).ToList();

        _context.CampusGroups.Remove(group);
        await _context.SaveChangesAsync();

        return courses;
    }
}