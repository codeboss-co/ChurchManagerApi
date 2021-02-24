using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DbMigrations.Migrations.Groups
{
    public partial class InitialGroupsDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Groups");

            migrationBuilder.CreateTable(
                name: "GroupMemberRole",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsLeader = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupType",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    GroupTerm = table.Column<string>(type: "text", nullable: true),
                    GroupMemberTerm = table.Column<string>(type: "text", nullable: true),
                    TakesAttendance = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentGroupId = table.Column<int>(type: "integer", nullable: true),
                    GroupTypeId = table.Column<int>(type: "integer", nullable: false),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    GroupCapacity = table.Column<int>(type: "integer", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Group_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Group_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalSchema: "Groups",
                        principalTable: "GroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAttendance",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DidNotOccur = table.Column<bool>(type: "boolean", nullable: true),
                    AttendanceCount = table.Column<int>(type: "integer", nullable: true),
                    FirstTimerCount = table.Column<int>(type: "integer", nullable: true),
                    NewConvertCount = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupAttendance_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    GroupMemberRoleId = table.Column<int>(type: "integer", nullable: false),
                    GroupMemberStatus = table.Column<string>(type: "text", nullable: true),
                    DateTimeAdded = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CommunicationPreference = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_GroupMemberRole_GroupMemberRoleId",
                        column: x => x.GroupMemberRoleId,
                        principalSchema: "Groups",
                        principalTable: "GroupMemberRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberAttendance",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupMemberId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DidAttend = table.Column<bool>(type: "boolean", nullable: true),
                    IsFirstTime = table.Column<bool>(type: "boolean", nullable: true),
                    IsNewConvert = table.Column<bool>(type: "boolean", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    GroupAttendanceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                        column: x => x.GroupAttendanceId,
                        principalSchema: "Groups",
                        principalTable: "GroupAttendance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_GroupMember_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalSchema: "Groups",
                        principalTable: "GroupMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupTypeId",
                schema: "Groups",
                table: "Group",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ParentGroupId",
                schema: "Groups",
                table: "Group",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAttendance_GroupId",
                schema: "Groups",
                table: "GroupAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId",
                schema: "Groups",
                table: "GroupMember",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupMemberRoleId",
                schema: "Groups",
                table: "GroupMember",
                column: "GroupMemberRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupAttendanceId",
                schema: "Groups",
                table: "GroupMemberAttendance",
                column: "GroupAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupId",
                schema: "Groups",
                table: "GroupMemberAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupMemberId",
                schema: "Groups",
                table: "GroupMemberAttendance",
                column: "GroupMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMemberAttendance",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupAttendance",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupMember",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "Group",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupMemberRole",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupType",
                schema: "Groups");
        }
    }
}
