using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class IconsCssGroupDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconCssClass",
                table: "GroupType",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconCssClass",
                table: "GroupType");
        }
    }
}
