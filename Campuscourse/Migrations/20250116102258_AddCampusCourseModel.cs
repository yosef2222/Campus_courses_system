using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campuscourse.Migrations
{
    /// <inheritdoc />
    public partial class AddCampusCourseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampusCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StartYear = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximumStudentsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentsEnrolledCount = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentsInQueueCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Requirements = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Annotations = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Semester = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampusCourseNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    IsImportant = table.Column<bool>(type: "INTEGER", nullable: false),
                    CampusCourseModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusCourseNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampusCourseNotifications_CampusCourses_CampusCourseModelId",
                        column: x => x.CampusCourseModelId,
                        principalTable: "CampusCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CampusCourseStudents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    MidtermResult = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalResult = table.Column<int>(type: "INTEGER", nullable: false),
                    CampusCourseModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusCourseStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampusCourseStudents_CampusCourses_CampusCourseModelId",
                        column: x => x.CampusCourseModelId,
                        principalTable: "CampusCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CampusCourseTeachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false),
                    CampusCourseModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusCourseTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampusCourseTeachers_CampusCourses_CampusCourseModelId",
                        column: x => x.CampusCourseModelId,
                        principalTable: "CampusCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseNotifications_CampusCourseModelId",
                table: "CampusCourseNotifications",
                column: "CampusCourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseStudents_CampusCourseModelId",
                table: "CampusCourseStudents",
                column: "CampusCourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseTeachers_CampusCourseModelId",
                table: "CampusCourseTeachers",
                column: "CampusCourseModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampusCourseNotifications");

            migrationBuilder.DropTable(
                name: "CampusCourseStudents");

            migrationBuilder.DropTable(
                name: "CampusCourseTeachers");

            migrationBuilder.DropTable(
                name: "CampusCourses");
        }
    }
}
