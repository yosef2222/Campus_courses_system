using Campuscourse.Data;
using Campuscourse.Models.CampusGroup;
using Campuscourse.Services.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampusGroupController : ControllerBase
{
    private readonly ICampusGroupService _campusGroupService;

    public CampusGroupController(ICampusGroupService campusGroupService)
    {
        _campusGroupService = campusGroupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _campusGroupService.GetAllGroupsAsync();
        return Ok(groups);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Group name cannot be empty.");

        var group = await _campusGroupService.CreateGroupAsync(request.Name);
        return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, group);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditGroup(Guid id, [FromBody] EditGroupRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Group name cannot be empty.");

        try
        {
            var updatedGroup = await _campusGroupService.EditGroupAsync(id, request.Name);
            return Ok(new { updatedGroup.Id, updatedGroup.Name });
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Group not found.");
        }
    }


    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        var courses = await _campusGroupService.DeleteGroupAsync(id);
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupById(Guid id)
    {
        try
        {
            var courses = await _campusGroupService.GetCoursesByGroupIdAsync(id);
            return Ok(courses);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}