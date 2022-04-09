using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class addprojectandtask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Project_ProjectId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Tasks",
                newName: "projectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_projectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Project_projectId",
                table: "Tasks",
                column: "projectId",
                principalTable: "Project",
                principalColumn: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Project_projectId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "projectId",
                table: "Tasks",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_projectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Project_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId");
        }
    }
}
