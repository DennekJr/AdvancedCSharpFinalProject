using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class Relationshipsfornotificationandtask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notification",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Notification",
                table: "Project");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOpned = table.Column<bool>(type: "bit", nullable: false),
                    projectId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    DeveloperTaskTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_Project_projectId",
                        column: x => x.projectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_Tasks_DeveloperTaskTaskId",
                        column: x => x.DeveloperTaskTaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DeveloperTaskTaskId",
                table: "Notification",
                column: "DeveloperTaskTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_projectId",
                table: "Notification",
                column: "projectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.AddColumn<string>(
                name: "Notification",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notification",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
