using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class AddHidden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DeveloperId",
                table: "Notification",
                column: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Developer_DeveloperId",
                table: "Notification",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "DeveloperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Developer_DeveloperId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_DeveloperId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "DevUserId",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeveloperId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
