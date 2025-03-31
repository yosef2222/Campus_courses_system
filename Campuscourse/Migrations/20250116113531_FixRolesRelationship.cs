using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Campuscourse.Migrations
{
    /// <inheritdoc />
    public partial class FixRolesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentsEnrolledCount",
                table: "CampusCourses");

            migrationBuilder.RenameColumn(
                name: "StudentsInQueueCount",
                table: "CampusCourses",
                newName: "MainTeacherId1");

            migrationBuilder.AddColumn<Guid>(
                name: "CampusGroupId",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MainTeacherId",
                table: "CampusCourses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_CampusGroupId",
                table: "CampusCourses",
                column: "CampusGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusCourses_MainTeacherId1",
                table: "CampusCourses",
                column: "MainTeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_CampusGroups_CampusGroupId",
                table: "CampusCourses",
                column: "CampusGroupId",
                principalTable: "CampusGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId1",
                table: "CampusCourses",
                column: "MainTeacherId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_CampusGroups_CampusGroupId",
                table: "CampusCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_CampusCourses_Users_MainTeacherId1",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_CampusGroupId",
                table: "CampusCourses");

            migrationBuilder.DropIndex(
                name: "IX_CampusCourses_MainTeacherId1",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "CampusGroupId",
                table: "CampusCourses");

            migrationBuilder.DropColumn(
                name: "MainTeacherId",
                table: "CampusCourses");

            migrationBuilder.RenameColumn(
                name: "MainTeacherId1",
                table: "CampusCourses",
                newName: "StudentsInQueueCount");

            migrationBuilder.AddColumn<int>(
                name: "StudentsEnrolledCount",
                table: "CampusCourses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
