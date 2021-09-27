using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class GroupAttendance_Cascade_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                table: "GroupMemberAttendance");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                table: "GroupMemberAttendance",
                column: "GroupAttendanceId",
                principalTable: "GroupAttendance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                table: "GroupMemberAttendance");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                table: "GroupMemberAttendance",
                column: "GroupAttendanceId",
                principalTable: "GroupAttendance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
