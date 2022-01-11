using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class UserLogin_Add_BaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveDateTime",
                table: "UserLogin",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecordStatus",
                table: "UserLogin",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InactiveDateTime",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "UserLogin");
        }
    }
}
