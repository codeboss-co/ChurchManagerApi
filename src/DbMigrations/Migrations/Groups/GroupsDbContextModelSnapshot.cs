﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DbMigrations.Migrations.Groups
{
    [DbContext(typeof(GroupsDbContext))]
    partial class GroupsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Groups")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Groups.Persistence.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ChurchId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("GroupCapacity")
                        .HasColumnType("integer");

                    b.Property<int>("GroupTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("ParentGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GroupTypeId");

                    b.HasIndex("ParentGroupId");

                    b.ToTable("Group", "Groups");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupAttendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AttendanceCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("AttendanceDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool?>("DidNotOccur")
                        .HasColumnType("boolean");

                    b.Property<int?>("FirstTimerCount")
                        .HasColumnType("integer");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int?>("NewConvertCount")
                        .HasColumnType("integer");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupAttendance", "Groups");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("ArchivedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CommunicationPreference")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateTimeAdded")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupMemberRoleId")
                        .HasColumnType("integer");

                    b.Property<string>("GroupMemberStatus")
                        .HasColumnType("text");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupMemberRoleId");

                    b.ToTable("GroupMember", "Groups");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupMemberAttendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("AttendanceDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool?>("DidAttend")
                        .HasColumnType("boolean");

                    b.Property<int?>("GroupAttendanceId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupMemberId")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsFirstTime")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsNewConvert")
                        .HasColumnType("boolean");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupAttendanceId");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupMemberId");

                    b.ToTable("GroupMemberAttendance", "Groups");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupMemberRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsLeader")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GroupMemberRole", "Groups");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("GroupMemberTerm")
                        .HasColumnType("text");

                    b.Property<string>("GroupTerm")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<bool>("TakesAttendance")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("GroupType", "Groups");
                });

            modelBuilder.Entity("Groups.Persistence.Models.Group", b =>
                {
                    b.HasOne("Groups.Persistence.Models.GroupType", "GroupType")
                        .WithMany()
                        .HasForeignKey("GroupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Groups.Persistence.Models.Group", "ParentGroup")
                        .WithMany("Groups")
                        .HasForeignKey("ParentGroupId");

                    b.Navigation("GroupType");

                    b.Navigation("ParentGroup");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupAttendance", b =>
                {
                    b.HasOne("Groups.Persistence.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupMember", b =>
                {
                    b.HasOne("Groups.Persistence.Models.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Groups.Persistence.Models.GroupMemberRole", "GroupMemberRole")
                        .WithMany()
                        .HasForeignKey("GroupMemberRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("GroupMemberRole");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupMemberAttendance", b =>
                {
                    b.HasOne("Groups.Persistence.Models.GroupAttendance", null)
                        .WithMany("Attendees")
                        .HasForeignKey("GroupAttendanceId");

                    b.HasOne("Groups.Persistence.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Groups.Persistence.Models.GroupMember", "GroupMember")
                        .WithMany()
                        .HasForeignKey("GroupMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("GroupMember");
                });

            modelBuilder.Entity("Groups.Persistence.Models.Group", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("Groups.Persistence.Models.GroupAttendance", b =>
                {
                    b.Navigation("Attendees");
                });
#pragma warning restore 612, 618
        }
    }
}
