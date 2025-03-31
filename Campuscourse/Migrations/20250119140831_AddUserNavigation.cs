using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campuscourse.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseStudents_Users_UserId",
                table: "CampusCourseStudents");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "CampusCourseTeachers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CampusCourseTeachers");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseTeachers_UserId",
                table: "CampusCourseTeachers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseStudents_Users_UserId",
                table: "CampusCourseStudents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseTeachers_Users_UserId",
                table: "CampusCourseTeachers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseStudents_Users_UserId",
                table: "CampusCourseStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseTeachers_Users_UserId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseTeachers_UserId",
                table: "CampusCourseTeachers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CampusCourseTeachers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CampusCourseTeachers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseStudents_Users_UserId",
                table: "CampusCourseStudents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
