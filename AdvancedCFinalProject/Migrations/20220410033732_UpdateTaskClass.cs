using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class UpdateTaskClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_ManagerId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ManagerId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
