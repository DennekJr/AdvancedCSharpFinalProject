using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCFinalProject.Migrations
{
    public partial class UpdateClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Tasks",
                newName: "CreatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Tasks",
                newName: "CreatedDate");
        }
    }
}
