using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class AttendanceRecord_Money : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Offering_Amount",
                table: "GroupAttendance",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offering_Currency",
                table: "GroupAttendance",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Offering_Amount",
                table: "GroupAttendance");

            migrationBuilder.DropColumn(
                name: "Offering_Currency",
                table: "GroupAttendance");
        }
    }
}
