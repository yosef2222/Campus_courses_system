﻿// <auto-generated />
using System;
using Campuscourse.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Campuscourse.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Campuscourse.Models.CampusCourse.CampusCourseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Annotations")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CampusGroupId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MainTeacherId")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaximumStudentsCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RemainingSlotsCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Requirements")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Semester")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StartYear")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CampusGroupId");

                    b.ToTable("CampusCourses");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourseNotification.CampusCourseNotificationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsImportant")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("CampusCourseNotifications");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourseStudent.CampusCourseStudentModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<int>("FinalResult")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MidtermResult")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("CampusCourseStudents");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourseTeacher.CampusCourseTeacherModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsMain")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("CampusCourseTeachers");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusGroup.CampusGroupModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CampusGroups");
                });

            modelBuilder.Entity("Campuscourse.Models.Roles.RolesModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTeacher")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Campuscourse.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Campuscourse.Models.Users.BlacklistedToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BlacklistedTokens");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourse.CampusCourseModel", b =>
                {
                    b.HasOne("Campuscourse.Models.CampusGroup.CampusGroupModel", "CampusGroup")
                        .WithMany("Courses")
                        .HasForeignKey("CampusGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CampusGroup");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourseNotification.CampusCourseNotificationModel", b =>
                {
                    b.HasOne("Campuscourse.Models.CampusCourse.CampusCourseModel", "Course")
                        .WithMany("Notifications")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourseStudent.CampusCourseStudentModel", b =>
                {
                    b.HasOne("Campuscourse.Models.CampusCourse.CampusCourseModel", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Campuscourse.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourseTeacher.CampusCourseTeacherModel", b =>
                {
                    b.HasOne("Campuscourse.Models.CampusCourse.CampusCourseModel", "Course")
                        .WithMany("Teachers")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Campuscourse.Models.UserModel", "User")
                        .WithMany("TeachingCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Campuscourse.Models.Roles.RolesModel", b =>
                {
                    b.HasOne("Campuscourse.Models.UserModel", "UserModel")
                        .WithOne("Role")
                        .HasForeignKey("Campuscourse.Models.Roles.RolesModel", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserModel");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusCourse.CampusCourseModel", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("Campuscourse.Models.CampusGroup.CampusGroupModel", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Campuscourse.Models.UserModel", b =>
                {
                    b.Navigation("Role")
                        .IsRequired();

                    b.Navigation("TeachingCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
