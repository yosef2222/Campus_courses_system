using Campuscourse.Data;
using Campuscourse.Models;
using Campuscourse.Models.CampusCourse;
using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campuscourse.Services.CampusCourse;

public class CampusCourseService : ICampusCourseService
{
    private readonly AppDbContext _context;

    public CampusCourseService(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task CreateCampusCourse(Guid groupId, CreateCampusCourseDto request)
    {
        var group = await _context.CampusGroups.FindAsync(groupId);
        if (group == null)
        {
            throw new KeyNotFoundException("Group not found.");
        }

        var teacher = await _context.Users.FindAsync(request.MainTeacherId);
        if (teacher == null)
        {
            throw new KeyNotFoundException("Main teacher not found.");
        }

        var course = new CampusCourseModel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            StartYear = request.StartYear,
            MaximumStudentsCount = request.MaximumStudentsCount,
            Semester = Enum.Parse<Semesters>(request.Semester, true),
            Requirements = request.Requirements,
            Annotations = request.Annotations,
            Status = CourseStatuses.Created,
            CampusGroupId = groupId,
            MainTeacherId = request.MainTeacherId,
            Teachers = new List<CampusCourseTeacherModel>
            {
                new CampusCourseTeacherModel
                {
                    UserId = teacher.Id,
                    IsMain = true
                }
            }
        };

        _context.CampusCourses.Add(course);

        await _context.SaveChangesAsync();
    }


    public async Task<List<CampusCourseResponseDto>> GetCoursesByGroup(Guid groupId)
    {
        return await _context.CampusCourses
            .Where(c => c.CampusGroupId == groupId)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Notifications)
            .Select(c => new CampusCourseResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                StartYear = c.StartYear,
                MaximumStudentsCount = c.MaximumStudentsCount,
                RemainingSlotsCount = c.MaximumStudentsCount - c.Students.Count,
                Status = c.Status.ToString(),
                Semester = c.Semester.ToString(),
                Students = c.Students.Select(s => new CampusCourseStudentResponseDto
                {
                    Id = s.User.Id,
                    Name = s.User.FullName ?? "N/A",
                    Email = s.User.Email ?? "N/A",
                    Status = s.Status.ToString(),
                    MidtermResult = s.MidtermResult.ToString(),
                    FinalResult = s.FinalResult.ToString()
                }).ToList(),
                Teachers = c.Teachers.Select(t => new CampusCourseTeacherResponseDto
                {
                    Name = t.User.FullName ?? "N/A",
                    Email = t.User.Email ?? "N/A",
                    IsMain = t.IsMain
                }).ToList(),
                Notifications = c.Notifications.Select(n => new CampusCourseNotificationResponseDto
                {
                    Text = n.Text ?? string.Empty,
                    IsImportant = n.IsImportant
                }).ToList()
            })
            .ToListAsync();
    }

    [Authorize(Policy = "TeacherOrAdmin")]
    public async Task UpdateRequirementsAndAnnotations(Guid courseId, UpdateCourseRequirementsDto request)
    {
        var course = await _context.CampusCourses.FindAsync(courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        course.Requirements = request.Requirements;
        course.Annotations = request.Annotations;

        _context.CampusCourses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task<CampusCourseResponseDto> GetCourseDetails(Guid courseId)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        return new CampusCourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            StudentsEnrolledCount = course.Students?.Count(s => s.Status == StudentStatuses.Accepted) ?? 0,
            StudentsInQueueCount = course.Students?.Count(s => s.Status == StudentStatuses.InQueue) ?? 0,
            Requirements = course.Requirements ?? string.Empty,
            Annotations = course.Annotations ?? string.Empty,
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString(),
            Students = course.Students?.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList() ?? new List<CampusCourseStudentResponseDto>(),
            Teachers = course.Teachers?.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList() ?? new List<CampusCourseTeacherResponseDto>(),
            Notifications = course.Notifications?.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList() ?? new List<CampusCourseNotificationResponseDto>()
        };
    }

    public async Task<PaginatedResponseDto<CampusCourseResponseDto>> GetCoursesList(
        int page = 1,
        int pageSize = 10,
        SortOrder sort = SortOrder.CreatedAsc,
        string search = null,
        bool? hasPlacesAndOpen = null,
        Semesters? semester = null)
    {
        var query = _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        if (hasPlacesAndOpen == true)
        {
            query = query.Where(c => c.Status == CourseStatuses.OpenForAssigning &&
                                     c.Students.Count(s => s.Status == StudentStatuses.Accepted) <
                                     c.MaximumStudentsCount);
        }

        if (semester.HasValue)
        {
            query = query.Where(c => c.Semester == semester.Value);
        }

        switch (sort)
        {
            case SortOrder.CreatedAsc:
                query = query.OrderBy(c => c.CreatedAt);
                break;
            case SortOrder.CreatedDesc:
                query = query.OrderByDescending(c => c.CreatedAt);
                break;
        }

        var totalCount = await query.CountAsync();

        var courses = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CampusCourseResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                StartYear = c.StartYear,
                MaximumStudentsCount = c.MaximumStudentsCount,
                StudentsEnrolledCount = c.Students.Count(s => s.Status == StudentStatuses.Accepted),
                StudentsInQueueCount = c.Students.Count(s => s.Status == StudentStatuses.InQueue),
                RemainingSlotsCount = c.MaximumStudentsCount - c.Students.Count(s => s.Status == StudentStatuses.Accepted),
                Requirements = c.Requirements,
                Annotations = c.Annotations,
                Status = c.Status.ToString(),
                Semester = c.Semester.ToString(),
                Students = c.Students.Select(s => new CampusCourseStudentResponseDto
                {
                    Id = s.User.Id,
                    Name = s.User.FullName ?? "N/A",
                    Email = s.User.Email ?? "N/A",
                    Status = s.Status.ToString(),
                    MidtermResult = s.MidtermResult.ToString(),
                    FinalResult = s.FinalResult.ToString()
                }).ToList(),
                Teachers = c.Teachers.Select(t => new CampusCourseTeacherResponseDto
                {
                    Name = t.User.FullName ?? "N/A",
                    Email = t.User.Email ?? "N/A",
                    IsMain = t.IsMain
                }).ToList(),
                Notifications = c.Notifications.Select(n => new CampusCourseNotificationResponseDto
                {
                    Text = n.Text ?? string.Empty,
                    IsImportant = n.IsImportant
                }).ToList()
            })
            .ToListAsync();

        return new PaginatedResponseDto<CampusCourseResponseDto>
        {
            Items = courses,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task UpdateCampusCourse(Guid id, UpdateCampusCourseDto request)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        course.Name = request.Name;
        course.StartYear = request.StartYear;
        course.MaximumStudentsCount = request.MaximumStudentsCount;
        course.Semester = Enum.Parse<Semesters>(request.Semester, true);
        course.Requirements = request.Requirements;
        course.Annotations = request.Annotations;
        course.MainTeacherId = request.MainTeacherId;

        if (!course.Teachers.Any(t => t.UserId == request.MainTeacherId && t.IsMain))
        {
            course.Teachers.Add(new CampusCourseTeacherModel
            {
                UserId = request.MainTeacherId,
                IsMain = true
            });
        }

        _context.CampusCourses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCampusCourse(Guid id)
    {
        var course = await _context.CampusCourses.FindAsync(id);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        _context.CampusCourses.Remove(course);
        await _context.SaveChangesAsync();
    }

    [Authorize(Policy = "TeacherOrAdmin")]
    public async Task<CampusCourseResponseDto> AddNotificationToCourse(Guid courseId, CreateNotificationDto request)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found or has been deleted.");
        }

        var notification = new CampusCourseNotificationModel
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Text = request.Text,
            IsImportant = request.IsImportant
        };

        _context.CampusCourseNotifications.Add(notification);

        await _context.SaveChangesAsync();

        var updatedCourse = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        return new CampusCourseResponseDto
        {
            Id = updatedCourse.Id,
            Name = updatedCourse.Name,
            StartYear = updatedCourse.StartYear,
            MaximumStudentsCount = updatedCourse.MaximumStudentsCount,
            StudentsEnrolledCount = updatedCourse.Students.Count(s => s.Status == StudentStatuses.Accepted),
            StudentsInQueueCount = updatedCourse.Students.Count(s => s.Status == StudentStatuses.InQueue),
            Requirements = updatedCourse.Requirements ?? string.Empty,
            Annotations = updatedCourse.Annotations ?? string.Empty,
            Status = updatedCourse.Status.ToString(),
            Semester = updatedCourse.Semester.ToString(),
            Students = updatedCourse.Students.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList(),
            Teachers = updatedCourse.Teachers.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList(),
            Notifications = updatedCourse.Notifications.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList()
        };
    }

    [Authorize(Policy = "TeacherOrAdmin")]
    public async Task<CampusCourseResponseDto> UpdateCourseStatus(Guid courseId, UpdateCourseStatusDto request)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if (!Enum.TryParse<CourseStatuses>(request.Status, true, out var newStatus))
        {
            throw new ArgumentException("Invalid course status.");
        }

        course.Status = newStatus;

        _context.CampusCourses.Update(course);
        await _context.SaveChangesAsync();

        return new CampusCourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            StudentsEnrolledCount = course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            StudentsInQueueCount = course.Students.Count(s => s.Status == StudentStatuses.InQueue),
            Requirements = course.Requirements ?? string.Empty,
            Annotations = course.Annotations ?? string.Empty,
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString(),
            Students = course.Students.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList(),
            Teachers = course.Teachers.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList(),
            Notifications = course.Notifications.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList()
        };
    }

    public async Task<CampusCourseResponseDto> SignUpForCourse(Guid courseId, Guid userId)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Students)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if (course.Status != CourseStatuses.OpenForAssigning)
        {
            throw new InvalidOperationException("Course is not open for assigning.");
        }

        if (course.Students.Any(s => s.UserId == userId))
        {
            throw new InvalidOperationException("User is already signed up for this course.");
        }

        var student = new CampusCourseStudentModel
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            UserId = userId,
            Status = StudentStatuses.InQueue
        };

        _context.CampusCourseStudents.Add(student);
        await _context.SaveChangesAsync();

        var updatedCourse = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        return new CampusCourseResponseDto
        {
            Id = updatedCourse.Id,
            Name = updatedCourse.Name,
            StartYear = updatedCourse.StartYear,
            MaximumStudentsCount = updatedCourse.MaximumStudentsCount,
            StudentsEnrolledCount = updatedCourse.Students.Count(s => s.Status == StudentStatuses.Accepted),
            StudentsInQueueCount = updatedCourse.Students.Count(s => s.Status == StudentStatuses.InQueue),
            Requirements = updatedCourse.Requirements ?? string.Empty,
            Annotations = updatedCourse.Annotations ?? string.Empty,
            Status = updatedCourse.Status.ToString(),
            Semester = updatedCourse.Semester.ToString(),
            Students = updatedCourse.Students.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList(),
            Teachers = updatedCourse.Teachers.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList(),
            Notifications = updatedCourse.Notifications.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList()
        };
    }


    [Authorize(Policy = "TeacherOrAdmin")]
    public async Task<CampusCourseResponseDto> UpdateStudentStatus(Guid courseId, Guid studentId,
        UpdateStudentStatusDto request)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        var student = course.Students.FirstOrDefault(s => s.UserId == studentId);

        if (student == null)
        {
            throw new KeyNotFoundException("Student not found in the course.");
        }

        if (!Enum.TryParse<StudentStatuses>(request.Status, true, out var newStatus))
        {
            throw new ArgumentException("Invalid student status.");
        }

        student.Status = newStatus;

        _context.CampusCourseStudents.Update(student);
        await _context.SaveChangesAsync();

        return new CampusCourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            StudentsEnrolledCount = course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            StudentsInQueueCount = course.Students.Count(s => s.Status == StudentStatuses.InQueue),
            Requirements = course.Requirements ?? string.Empty,
            Annotations = course.Annotations ?? string.Empty,
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString(),
            Students = course.Students.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList(),
            Teachers = course.Teachers.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList(),
            Notifications = course.Notifications.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList()
        };
    }


    [Authorize(Policy = "TeacherOrAdmin")]
    public async Task<CampusCourseResponseDto> UpdateStudentMark(Guid courseId, Guid studentId,
        UpdateStudentMarkDto request)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        var student = course.Students.FirstOrDefault(s => s.UserId == studentId);

        if (student == null)
        {
            throw new KeyNotFoundException("Student not found in the course.");
        }

        if (!Enum.TryParse<StudentMarks>(request.Mark, true, out var newMark))
        {
            throw new ArgumentException("Invalid mark value.");
        }

        if (request.MarkType.Equals("Midterm", StringComparison.OrdinalIgnoreCase))
        {
            student.MidtermResult = newMark;
        }
        else if (request.MarkType.Equals("Final", StringComparison.OrdinalIgnoreCase))
        {
            student.FinalResult = newMark;
        }
        else
        {
            throw new ArgumentException("Invalid mark type. Allowed values are 'Midterm' or 'Final'.");
        }

        _context.CampusCourseStudents.Update(student);
        await _context.SaveChangesAsync();

        return new CampusCourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            StudentsEnrolledCount = course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            StudentsInQueueCount = course.Students.Count(s => s.Status == StudentStatuses.InQueue),
            Requirements = course.Requirements ?? string.Empty,
            Annotations = course.Annotations ?? string.Empty,
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString(),
            Students = course.Students.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList(),
            Teachers = course.Teachers.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList(),
            Notifications = course.Notifications.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList()
        };
    }

    public async Task<List<CampusCourseResponseDto>> GetUserCourses(Guid userId)
    {
        var courses = await _context.CampusCourseStudents
            .Where(cs =>
                cs.UserId == userId && cs.Status == StudentStatuses.Accepted)
            .Select(cs => cs.Course)
            .Include(c => c.Students)
            .ToListAsync();

        return courses.Select(course => new CampusCourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            RemainingSlotsCount = course.MaximumStudentsCount -
                                  course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString()
        }).ToList();
    }

    public async Task<List<CampusCourseResponseAsTeacherDto>> GetTeachingCourses(Guid userId)
    {
        var courses = await _context.CampusCourseTeachers
            .Where(ct => ct.UserId == userId)
            .Select(ct => ct.Course)
            .Distinct()
            .Include(c => c.Students)
            .ToListAsync();

        return courses.Select(course => new CampusCourseResponseAsTeacherDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            RemainingSlotsCount = course.MaximumStudentsCount -
                                  course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString()
        }).ToList();
    }

    public async Task<CampusCourseResponseDto> AddTeacherToCourse(Guid courseId, AddTeacherToCourseDto request)
    {
        var course = await _context.CampusCourses
            .Include(c => c.Teachers)
            .ThenInclude(ct => ct.User)
            .Include(c => c.Students)
            .ThenInclude(cs => cs.User)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (course.Teachers.Any(t => t.UserId == request.UserId))
        {
            throw new InvalidOperationException("User is already a teacher for this course.");
        }

        var teacher = new CampusCourseTeacherModel
        {
            CourseId = courseId,
            UserId = request.UserId,
            IsMain = false
        };

        _context.CampusCourseTeachers.Add(teacher);
        await _context.SaveChangesAsync();

        return new CampusCourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartYear = course.StartYear,
            MaximumStudentsCount = course.MaximumStudentsCount,
            StudentsEnrolledCount = course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            StudentsInQueueCount = course.Students.Count(s => s.Status == StudentStatuses.InQueue),
            Requirements = course.Requirements ?? string.Empty,
            Annotations = course.Annotations ?? string.Empty,
            Status = course.Status.ToString(),
            Semester = course.Semester.ToString(),
            Students = course.Students.Select(s => new CampusCourseStudentResponseDto
            {
                Id = s.User.Id,
                Name = s.User.FullName ?? "N/A",
                Email = s.User.Email ?? "N/A",
                Status = s.Status.ToString(),
                MidtermResult = s.MidtermResult.ToString(),
                FinalResult = s.FinalResult.ToString()
            }).ToList(),
            Teachers = course.Teachers.Select(t => new CampusCourseTeacherResponseDto
            {
                Name = t.User.FullName ?? "N/A",
                Email = t.User.Email ?? "N/A",
                IsMain = t.IsMain
            }).ToList(),
            Notifications = course.Notifications.Select(n => new CampusCourseNotificationResponseDto
            {
                Text = n.Text ?? string.Empty,
                IsImportant = n.IsImportant
            }).ToList()
        };
    }
}