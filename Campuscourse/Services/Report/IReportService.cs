using Campuscourse.Models.Reports;

namespace Campuscourse.Services.Report;

public interface IReportService
{
    Task<List<CampusGroupReportDto>> GenerateReport(string? semester, List<Guid>? campusGroupIds);
}