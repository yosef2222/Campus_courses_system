using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campuscourse.Migrations
{
    /// <inheritdoc />
    public partial class AddEditCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_MainTeacherId",
                table: "CampusCourses");

            migrationBuilder.AlterColumn<int>(
                name: "Semester",
                table: "CampusCourses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Semester",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_MainTeacherId",
                table: "CampusCourses",
                column: "MainTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId",
                table: "CampusCourses",
                column: "MainTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
