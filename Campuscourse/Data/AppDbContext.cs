using Microsoft.EntityFrameworkCore;
using Campuscourse.Models;
using Campuscourse.Models.CampusCourse;
using Campuscourse.Models.CampusCourseNotification;
using Campuscourse.Models.CampusCourseStudent;
using Campuscourse.Models.CampusCourseTeacher;
using Campuscourse.Models.CampusGroup;
using Campuscourse.Models.Roles;
using Campuscourse.Models.Users;

namespace Campuscourse.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RolesModel> Roles { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
        public DbSet<CampusGroupModel> CampusGroups { get; set; }
        public DbSet<CampusCourseModel> CampusCourses { get; set; }
        public DbSet<CampusCourseStudentModel> CampusCourseStudents { get; set; }
        public DbSet<CampusCourseTeacherModel> CampusCourseTeachers { get; set; }
        public DbSet<CampusCourseNotificationModel> CampusCourseNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasOne(u => u.Role)
                .WithOne(r => r.UserModel)
                .HasForeignKey<RolesModel>(r => r.UserId);

            modelBuilder.Entity<CampusCourseStudentModel>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(cs => cs.CourseId);

            modelBuilder.Entity<CampusCourseStudentModel>()
                .HasOne(cs => cs.User)
                .WithMany()
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampusCourseModel>()
                .HasOne(c => c.CampusGroup)
                .WithMany(g => g.Courses)
                .HasForeignKey(c => c.CampusGroupId);

            modelBuilder.Entity<CampusCourseTeacherModel>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.Teachers)
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampusCourseTeacherModel>()
                .HasOne(ct => ct.User)
                .WithMany()
                .HasForeignKey(ct => ct.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampusCourseNotificationModel>()
                .HasOne(n => n.Course)
                .WithMany(c => c.Notifications)
                .HasForeignKey(n => n.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampusCourseTeacherModel>()
                .HasOne(ct => ct.User)
                .WithMany(u => u.TeachingCourses)
                .HasForeignKey(ct => ct.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampusCourseTeacherModel>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.Teachers)
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampusCourseModel>()
                .HasMany(c => c.Students)
                .WithOne(cs => cs.Course)
                .HasForeignKey(cs => cs.CourseId);

            modelBuilder.Entity<CampusCourseModel>()
                .HasMany(c => c.Teachers)
                .WithOne(ct => ct.Course)
                .HasForeignKey(ct => ct.CourseId);

            modelBuilder.Entity<CampusCourseModel>()
                .HasMany(c => c.Notifications)
                .WithOne(n => n.Course)
                .HasForeignKey(n => n.CourseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}