using Campuscourse.Data;
using Campuscourse.Models.CampusCourse;
using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.Enums;
using Campuscourse.Services.CampusCourse;
using Campuscourse.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampusCourseController : ControllerBase
{
    private readonly ICampusCourseService _campusCourseService;
    private readonly IUserService _userService;
    public CampusCourseController(ICampusCourseService campusCourseService, IUserService userService)
    {
        _campusCourseService = campusCourseService;
        _userService = userService;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{groupId}")]
    public async Task<IActionResult> CreateCampusCourse(Guid groupId, CreateCampusCourseDto request)
    {
        var validationErrors = new Dictionary<string, List<string>>();

        if (request.StartYear < 2000 || request.StartYear > 2029)
        {
            if (!validationErrors.ContainsKey("StartYear"))
            {
                validationErrors["StartYear"] = new List<string>();
            }
            validationErrors["StartYear"].Add("Campus course start year must be between 2000 and 2029.");
        }

        if (request.MaximumStudentsCount < 1 || request.MaximumStudentsCount > 200)
        {
            if (!validationErrors.ContainsKey("MaximumStudentsCount"))
            {
                validationErrors["MaximumStudentsCount"] = new List<string>();
            }
            validationErrors["MaximumStudentsCount"].Add("Maximum student count must be between 1 and 200.");
        }

        if (validationErrors.Any())
        {
            return BadRequest(new { errors = validationErrors });
        }

        try
        {
            await _campusCourseService.CreateCampusCourse(groupId, request);
            return Ok("Campus course created successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [Authorize]
    [HttpPut("{id}/requirements-and-annotations")]
    public async Task<IActionResult> UpdateRequirementsAndAnnotations(Guid id,
        [FromBody] UpdateCourseRequirementsDto request)
    {
        try
        {
            await _campusCourseService.UpdateRequirementsAndAnnotations(id, request);

            var updatedCourse = await _campusCourseService.GetCourseDetails(id);

            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCoursesList(
        [FromQuery] SortOrder sort = SortOrder.CreatedAsc,
        [FromQuery] string search = null,
        [FromQuery] bool? hasPlacesAndOpen = null,
        [FromQuery] Semesters? semester = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _campusCourseService.GetCoursesList(
                page, pageSize, sort, search, hasPlacesAndOpen, semester);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCampusCourse(Guid id, [FromBody] UpdateCampusCourseDto request)
    {
        try
        {
            await _campusCourseService.UpdateCampusCourse(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCampusCourse(Guid id)
    {
        try
        {
            await _campusCourseService.DeleteCampusCourse(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetCampusCourseDetails(Guid id)
    {
        try
        {
            var courseDetails = await _campusCourseService.GetCourseDetails(id);
            return Ok(courseDetails);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{id}/notifications")]
    public async Task<IActionResult> AddNotification(Guid id, [FromBody] CreateNotificationDto request)
    {
        try
        {
            var updatedCourse = await _campusCourseService.AddNotificationToCourse(id, request);
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{id}/status")]
    public async Task<IActionResult> UpdateCourseStatus(Guid id, [FromBody] UpdateCourseStatusDto request)
    {
        try
        {
            var updatedCourse = await _campusCourseService.UpdateCourseStatus(id, request);
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{id}/sign-up")]
    public async Task<IActionResult> SignUpForCourse(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var updatedCourse = await _campusCourseService.SignUpForCourse(id, userId);
            await _userService.GrantRole(userId, "Student");
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{id}/student-status/{studentId}")]
    public async Task<IActionResult> UpdateStudentStatus(Guid id, Guid studentId,
        [FromBody] UpdateStudentStatusDto request)
    {
        try
        {
            var updatedCourse = await _campusCourseService.UpdateStudentStatus(id, studentId, request);
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{id}/marks/{studentId}")]
    public async Task<IActionResult> UpdateStudentMark(Guid id, Guid studentId, [FromBody] UpdateStudentMarkDto request)
    {
        try
        {
            var updatedCourse = await _campusCourseService.UpdateStudentMark(id, studentId, request);
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyCourses()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst("Id")?.Value);

            var courses = await _campusCourseService.GetUserCourses(userId);
            return Ok(courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("teaching")]
    public async Task<IActionResult> GetTeachingCourses()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst("Id")?.Value);

            var courses = await _campusCourseService.GetTeachingCourses(userId);
            return Ok(courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{id}/teachers")]
    public async Task<IActionResult> AddTeacherToCourse(Guid id, [FromBody] AddTeacherToCourseDto request)
    {
        try
        {
            var updatedCourse = await _campusCourseService.AddTeacherToCourse(id, request);
            await _userService.GrantRole(id, "Teacher");
            return Ok(updatedCourse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}