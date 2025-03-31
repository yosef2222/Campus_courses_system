using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campuscourse.Migrations
{
    /// <inheritdoc />
    public partial class FixCC1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseNotifications_CampusCourses_CampusCourseModelId",
                table: "CampusCourseNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId1",
                table: "CampusCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseStudents_CampusCourses_CampusCourseModelId",
                table: "CampusCourseStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseTeachers_CampusCourses_CampusCourseModelId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseTeachers_CampusCourseModelId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseStudents_CampusCourseModelId",
                table: "CampusCourseStudents");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_MainTeacherId1",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseNotifications_CampusCourseModelId",
                table: "CampusCourseNotifications");

            migrationBuilder.DropColumn(
                name: "CampusCourseModelId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropColumn(
                name: "CampusCourseModelId",
                table: "CampusCourseStudents");

            migrationBuilder.DropColumn(
                name: "CampusCourseModelId",
                table: "CampusCourseNotifications");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CampusCourseStudents",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "CampusCourseStudents",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "MainTeacherId1",
                table: "CampusCourses",
                newName: "MidtermResult");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "CampusCourseTeachers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CampusCourseTeachers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "CampusCourseNotifications",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "BlacklistedTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseTeachers_CourseId",
                table: "CampusCourseTeachers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseStudents_CourseId",
                table: "CampusCourseStudents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseStudents_UserId",
                table: "CampusCourseStudents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_CourseId",
                table: "CampusCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_MainTeacherId",
                table: "CampusCourses",
                column: "MainTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_UserId",
                table: "CampusCourses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseNotifications_CourseId",
                table: "CampusCourseNotifications",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseNotifications_CampusCourses_CourseId",
                table: "CampusCourseNotifications",
                column: "CourseId",
                principalTable: "CampusCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_CampusCourses_CourseId",
                table: "CampusCourses",
                column: "CourseId",
                principalTable: "CampusCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId",
                table: "CampusCourses",
                column: "MainTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_Users_UserId",
                table: "CampusCourses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseStudents_CampusCourses_CourseId",
                table: "CampusCourseStudents",
                column: "CourseId",
                principalTable: "CampusCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseStudents_Users_UserId",
                table: "CampusCourseStudents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseTeachers_CampusCourses_CourseId",
                table: "CampusCourseTeachers",
                column: "CourseId",
                principalTable: "CampusCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseNotifications_CampusCourses_CourseId",
                table: "CampusCourseNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_CampusCourses_CourseId",
                table: "CampusCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId",
                table: "CampusCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_Users_UserId",
                table: "CampusCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseStudents_CampusCourses_CourseId",
                table: "CampusCourseStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseStudents_Users_UserId",
                table: "CampusCourseStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourseTeachers_CampusCourses_CourseId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseTeachers_CourseId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseStudents_CourseId",
                table: "CampusCourseStudents");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseStudents_UserId",
                table: "CampusCourseStudents");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_CourseId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_MainTeacherId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_UserId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourseNotifications_CourseId",
                table: "CampusCourseNotifications");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CampusCourseTeachers");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "FinalResult",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CampusCourseNotifications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CampusCourseStudents",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CampusCourseStudents",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "MidtermResult",
                table: "CampusCourses",
                newName: "MainTeacherId1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "CampusCourseModelId",
                table: "CampusCourseTeachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CampusCourseModelId",
                table: "CampusCourseStudents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CampusCourseModelId",
                table: "CampusCourseNotifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlacklistedTokens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseTeachers_CampusCourseModelId",
                table: "CampusCourseTeachers",
                column: "CampusCourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseStudents_CampusCourseModelId",
                table: "CampusCourseStudents",
                column: "CampusCourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_MainTeacherId1",
                table: "CampusCourses",
                column: "MainTeacherId1");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourseNotifications_CampusCourseModelId",
                table: "CampusCourseNotifications",
                column: "CampusCourseModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseNotifications_CampusCourses_CampusCourseModelId",
                table: "CampusCourseNotifications",
                column: "CampusCourseModelId",
                principalTable: "CampusCourses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId1",
                table: "CampusCourses",
                column: "MainTeacherId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseStudents_CampusCourses_CampusCourseModelId",
                table: "CampusCourseStudents",
                column: "CampusCourseModelId",
                principalTable: "CampusCourses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourseTeachers_CampusCourses_CampusCourseModelId",
                table: "CampusCourseTeachers",
                column: "CampusCourseModelId",
                principalTable: "CampusCourses",
                principalColumn: "Id");
        }
    }
}
