using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class PushDevice_UniqueIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentification",
                table: "PushDevice",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueIdentification",
                table: "PushDevice");
        }
    }
}
