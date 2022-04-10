using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class CheckForMissing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DevUserId",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DevUserId",
                table: "Notification",
                column: "DevUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DeveloperId",
                table: "AspNetUsers",
                column: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Developer_DeveloperId",
                table: "AspNetUsers",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_DevUserId",
                table: "Notification",
                column: "DevUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Developer_DeveloperId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_DevUserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_DevUserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DeveloperId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DevUserId",
                table: "Notification");
        }
    }
}
