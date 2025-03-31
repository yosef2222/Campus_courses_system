using Campuscourse.Data;
using Campuscourse.Models.Enums;
using Campuscourse.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Controllers;
[Authorize(Policy = "AdminOnly")]
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] string? semester, [FromQuery] List<Guid>? campusGroupIds)
    {
        var report = await _reportService.GenerateReport(semester, campusGroupIds);
        return Ok(report);
    }
}

