using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campuscourse.Migrations
{
    /// <inheritdoc />
    public partial class AddCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_CampusCourses_CourseId",
                table: "CampusCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_Users_UserId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_CourseId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_UserId",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "FinalResult",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "MidtermResult",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CampusCourses");

            migrationBuilder.AlterColumn<string>(
                name: "Semester",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Semester",
                table: "CampusCourses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "FinalResult",
                table: "CampusCourses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MidtermResult",
                table: "CampusCourses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_CourseId",
                table: "CampusCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_UserId",
                table: "CampusCourses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_CampusCourses_CourseId",
                table: "CampusCourses",
                column: "CourseId",
                principalTable: "CampusCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_Users_UserId",
                table: "CampusCourses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
