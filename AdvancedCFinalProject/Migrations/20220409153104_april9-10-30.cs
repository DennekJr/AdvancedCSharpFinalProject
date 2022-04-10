using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class april91030 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Developer_DeveloperId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_DeveloperTaskId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DeveloperTaskId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TaskId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperTaskId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsURgent",
                table: "Comments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrgentNote",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeveloperTaskId",
                table: "Comments",
                column: "DeveloperTaskId",
                unique: true,
                filter: "[DeveloperTaskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskId",
                table: "Comments",
                column: "TaskId",
                unique: true,
                filter: "[TaskId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Developer_DeveloperId",
                table: "Comments",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_DeveloperTaskId",
                table: "Comments",
                column: "DeveloperTaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Developer_DeveloperId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_DeveloperTaskId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DeveloperTaskId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TaskId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsURgent",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UrgentNote",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperTaskId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeveloperTaskId",
                table: "Comments",
                column: "DeveloperTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskId",
                table: "Comments",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Developer_DeveloperId",
                table: "Comments",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "DeveloperId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_DeveloperTaskId",
                table: "Comments",
                column: "DeveloperTaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
