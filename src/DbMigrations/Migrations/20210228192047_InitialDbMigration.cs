using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DbMigrations.Migrations
{
    public partial class InitialDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Churches");

            migrationBuilder.EnsureSchema(
                name: "People");

            migrationBuilder.EnsureSchema(
                name: "Groups");

            migrationBuilder.CreateTable(
                name: "ChurchGroup",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LeaderPersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Family",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberRole",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GroupTerm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GroupMemberTerm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TakesAttendance = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Church",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchGroupId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShortCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LeaderPersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Church", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Church_ChurchGroup_ChurchGroupId",
                        column: x => x.ChurchGroupId,
                        principalSchema: "Churches",
                        principalTable: "ChurchGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GroupCapacity = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Person",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    ConnectionStatus = table.Column<string>(type: "text", nullable: true),
                    DeceasedStatus_IsDeceased = table.Column<bool>(type: "boolean", nullable: true),
                    DeceasedStatus_DeceasedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AgeClassification = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    FirstVisitDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FullName_Title = table.Column<string>(type: "text", nullable: true),
                    FullName_FirstName = table.Column<string>(type: "text", nullable: true),
                    FullName_NickName = table.Column<string>(type: "text", nullable: true),
                    FullName_MiddleName = table.Column<string>(type: "text", nullable: true),
                    FullName_LastName = table.Column<string>(type: "text", nullable: true),
                    FullName_Suffix = table.Column<string>(type: "text", nullable: true),
                    BirthDate_BirthDay = table.Column<int>(type: "integer", nullable: true),
                    BirthDate_BirthMonth = table.Column<int>(type: "integer", nullable: true),
                    BirthDate_BirthYear = table.Column<int>(type: "integer", nullable: true),
                    BaptismStatus_IsBaptised = table.Column<bool>(type: "boolean", nullable: true),
                    BaptismStatus_BaptismDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    AnniversaryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Email_Address = table.Column<string>(type: "text", nullable: true),
                    Email_IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CommunicationPreference = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    FamilyId = table.Column<int>(type: "integer", nullable: true),
                    ReceivedHolySpirit = table.Column<bool>(type: "boolean", nullable: true),
                    GivingGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserLoginId = table.Column<string>(type: "text", nullable: true),
                    ViewedCount = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "People",
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
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
                    ArchiveStatus_IsArchived = table.Column<bool>(type: "boolean", nullable: true),
                    ArchiveStatus_ArchivedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CommunicationPreference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RecordStatus = table.Column<string>(type: "text", nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
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
                    table.ForeignKey(
                        name: "FK_GroupMember_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsMessagingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsUnlisted = table.Column<bool>(type: "boolean", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                name: "IX_Church_ChurchGroupId",
                schema: "Churches",
                table: "Church",
                column: "ChurchGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ChurchId",
                schema: "Groups",
                table: "Group",
                column: "ChurchId");

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
                name: "IX_GroupMember_PersonId",
                schema: "Groups",
                table: "GroupMember",
                column: "PersonId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Person_ChurchId",
                schema: "People",
                table: "Person",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FamilyId",
                schema: "People",
                table: "Person",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_PersonId",
                schema: "People",
                table: "PhoneNumber",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMemberAttendance",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "PhoneNumber",
                schema: "People");

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
                name: "Person",
                schema: "People");

            migrationBuilder.DropTable(
                name: "GroupType",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "Church",
                schema: "Churches");

            migrationBuilder.DropTable(
                name: "Family",
                schema: "People");

            migrationBuilder.DropTable(
                name: "ChurchGroup",
                schema: "Churches");
        }
    }
}
