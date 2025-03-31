using Campuscourse.Models;
using Campuscourse.Models.CampusCourse;
using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Campuscourse.Services.CampusCourse;

public interface ICampusCourseService
{
    Task CreateCampusCourse(Guid groupId, CreateCampusCourseDto request);
    Task<List<CampusCourseResponseDto>> GetCoursesByGroup(Guid groupId);
    Task UpdateRequirementsAndAnnotations(Guid courseId, UpdateCourseRequirementsDto request);
    Task<CampusCourseResponseDto> GetCourseDetails(Guid courseId);

    Task<PaginatedResponseDto<CampusCourseResponseDto>> GetCoursesList(
        int page = 1,
        int pageSize = 10,
        SortOrder sort = SortOrder.CreatedAsc,
        string search = null,
        bool? hasPlacesAndOpen = null,
        Semesters? semester = null);

    Task UpdateCampusCourse(Guid id,
        UpdateCampusCourseDto request);

    Task DeleteCampusCourse(Guid id);
    Task<CampusCourseResponseDto> AddNotificationToCourse(Guid courseId, CreateNotificationDto request);
    Task<CampusCourseResponseDto> UpdateCourseStatus(Guid courseId, UpdateCourseStatusDto request);
    Task<CampusCourseResponseDto> SignUpForCourse(Guid courseId, Guid userId);
    Task<CampusCourseResponseDto> UpdateStudentStatus(Guid courseId, Guid studentId, UpdateStudentStatusDto request);
    Task<CampusCourseResponseDto> UpdateStudentMark(Guid courseId, Guid studentId, UpdateStudentMarkDto request);
    Task<List<CampusCourseResponseDto>> GetUserCourses(Guid userId);
    Task<List<CampusCourseResponseAsTeacherDto>> GetTeachingCourses(Guid userId);
    Task<CampusCourseResponseDto> AddTeacherToCourse(Guid courseId, AddTeacherToCourseDto request);
}