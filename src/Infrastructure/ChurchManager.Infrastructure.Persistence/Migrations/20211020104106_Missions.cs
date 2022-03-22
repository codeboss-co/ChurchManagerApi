using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class Missions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IconCssClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    Attendance_AttendanceCount = table.Column<int>(type: "integer", nullable: true),
                    Attendance_FirstTimerCount = table.Column<int>(type: "integer", nullable: true),
                    Attendance_NewConvertCount = table.Column<int>(type: "integer", nullable: true),
                    Attendance_ReceivedHolySpiritCount = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PhotoUrls = table.Column<List<string>>(type: "text[]", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mission_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mission_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mission_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mission_ChurchId",
                table: "Mission",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_GroupId",
                table: "Mission",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_PersonId",
                table: "Mission",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mission");
        }
    }
}
