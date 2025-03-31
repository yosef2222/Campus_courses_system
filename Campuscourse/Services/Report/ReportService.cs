using Campuscourse.Data;
using Campuscourse.Models.Enums;
using Campuscourse.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Services.Report;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CampusGroupReportDto>> GenerateReport(string? semester, List<Guid>? campusGroupIds)
    {
        var query = _context.CampusCourses
            .Include(c => c.CampusGroup)
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(semester))
        {
            if (!Enum.TryParse<Semesters>(semester, true, out var semesterEnum))
            {
                throw new ArgumentException("Invalid semester value.");
            }

            query = query.Where(c => c.Semester == semesterEnum);
        }

        if (campusGroupIds != null && campusGroupIds.Any())
        {
            query = query.Where(c => campusGroupIds.Contains(c.CampusGroupId));
        }

        var courses = await query.ToListAsync();

        var report = courses
            .GroupBy(c => c.CampusGroup) // Group by CampusGroup
            .Select(g => new CampusGroupReportDto
            {
                Name = g.Key.Name,
                Id = g.Key.Id,
                AveragePassed = g.SelectMany(c => c.Students).Any()
                    ? g.SelectMany(c => c.Students)
                          .Count(s => s.FinalResult == StudentMarks.Passed) /
                      (double)g.SelectMany(c => c.Students).Count()
                    : 0,
                AverageFailed = g.SelectMany(c => c.Students).Any()
                    ? g.SelectMany(c => c.Students)
                          .Count(s => s.FinalResult == StudentMarks.Failed) /
                      (double)g.SelectMany(c => c.Students).Count()
                    : 0
            })
            .ToList();
        
        return report;
    }
}