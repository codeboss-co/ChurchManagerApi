using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class Mission_Stream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stream",
                table: "Mission",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stream",
                table: "Mission");
        }
    }
}
