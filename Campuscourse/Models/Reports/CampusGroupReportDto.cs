namespace Campuscourse.Models.Reports;

public class CampusGroupReportDto
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public double AveragePassed { get; set; }
    public double AverageFailed { get; set; }
}