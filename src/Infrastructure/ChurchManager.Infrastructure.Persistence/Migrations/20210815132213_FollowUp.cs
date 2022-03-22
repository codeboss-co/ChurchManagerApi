using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    public partial class FollowUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssignedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    AssignedPersonId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    RequiresAdditionalFollowUp = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUp_Person_AssignedPersonId",
                        column: x => x.AssignedPersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUp_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_AssignedPersonId",
                table: "FollowUp",
                column: "AssignedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_PersonId",
                table: "FollowUp",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUp");
        }
    }
}
