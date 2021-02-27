﻿// <auto-generated />
using System;
using DbMigrations.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DbMigrations.Migrations
{
    [DbContext(typeof(ChurchManagerDbContext))]
    [Migration("20210227140415_InitialDbMigration")]
    partial class InitialDbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Churches.Persistence.Models.Church", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int?>("ChurchGroupId")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("LeaderPersonId")
                        .HasColumnType("integer");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RecordStatus")
                        .HasColumnType("text");

                    b.Property<string>("ShortCode")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("ChurchGroupId");

                    b.ToTable("Church", "Churches");
                });

            modelBuilder.Entity("Churches.Persistence.Models.ChurchGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("LeaderPersonId")
                        .HasColumnType("integer");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RecordStatus")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ChurchGroup", "Churches");
                });

            modelBuilder.Entity("Groups.Persistence.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ChurchId")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("GroupCapacity")
                        .HasColumnType("integer");

                    b.Property<int>("GroupTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("ParentGroupId")
                        .HasColumnType("integer");

                    b.Property<string>("RecordStatus")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChurchId");

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
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

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

                    b.Property<string>("CommunicationPreference")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupMemberRoleId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<string>("RecordStatus")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupMemberRoleId");

                    b.HasIndex("PersonId");

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
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

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
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsLeader")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

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
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("GroupMemberTerm")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("GroupTerm")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("TakesAttendance")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("GroupType", "Groups");
                });

            modelBuilder.Entity("People.Persistence.Models.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RecordStatus")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Family", "People");
                });

            modelBuilder.Entity("People.Persistence.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AgeClassification")
                        .HasColumnType("text");

                    b.Property<DateTime?>("AnniversaryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ChurchId")
                        .HasColumnType("integer");

                    b.Property<string>("CommunicationPreference")
                        .HasColumnType("text");

                    b.Property<string>("ConnectionStatus")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("FamilyId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FirstVisitDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<Guid?>("GivingGroupId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("InactiveDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("RecordStatus")
                        .HasColumnType("text");

                    b.Property<string>("UserLoginId")
                        .HasColumnType("text");

                    b.Property<int?>("ViewedCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ChurchId");

                    b.HasIndex("FamilyId");

                    b.ToTable("Person", "People");
                });

            modelBuilder.Entity("People.Persistence.Models.PhoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CountryCode")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<bool>("IsMessagingEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUnlisted")
                        .HasColumnType("boolean");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<int?>("PersonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("PhoneNumber", "People");
                });

            modelBuilder.Entity("Churches.Persistence.Models.Church", b =>
                {
                    b.HasOne("Churches.Persistence.Models.ChurchGroup", "ChurchGroup")
                        .WithMany("Churches")
                        .HasForeignKey("ChurchGroupId");

                    b.Navigation("ChurchGroup");
                });

            modelBuilder.Entity("Groups.Persistence.Models.Group", b =>
                {
                    b.HasOne("Churches.Persistence.Models.Church", "Church")
                        .WithMany()
                        .HasForeignKey("ChurchId");

                    b.HasOne("Groups.Persistence.Models.GroupType", "GroupType")
                        .WithMany()
                        .HasForeignKey("GroupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Groups.Persistence.Models.Group", "ParentGroup")
                        .WithMany("Groups")
                        .HasForeignKey("ParentGroupId");

                    b.Navigation("Church");

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

                    b.HasOne("People.Persistence.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Groups.Persistence.Models.ArchiveStatus", "ArchiveStatus", b1 =>
                        {
                            b1.Property<int>("GroupMemberId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<DateTime?>("ArchivedDateTime")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<bool?>("IsArchived")
                                .HasColumnType("boolean");

                            b1.HasKey("GroupMemberId");

                            b1.ToTable("GroupMember");

                            b1.WithOwner()
                                .HasForeignKey("GroupMemberId");
                        });

                    b.Navigation("ArchiveStatus");

                    b.Navigation("Group");

                    b.Navigation("GroupMemberRole");

                    b.Navigation("Person");
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

            modelBuilder.Entity("People.Persistence.Models.Person", b =>
                {
                    b.HasOne("Churches.Persistence.Models.Church", "Church")
                        .WithMany()
                        .HasForeignKey("ChurchId");

                    b.HasOne("People.Persistence.Models.Family", "Family")
                        .WithMany("FamilyMembers")
                        .HasForeignKey("FamilyId");

                    b.OwnsOne("People.Persistence.Models.Baptism", "BaptismStatus", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<DateTime?>("BaptismDate")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<bool?>("IsBaptised")
                                .HasColumnType("boolean");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("People.Persistence.Models.BirthDate", "BirthDate", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<int?>("BirthDay")
                                .HasColumnType("integer");

                            b1.Property<int?>("BirthMonth")
                                .HasColumnType("integer");

                            b1.Property<int?>("BirthYear")
                                .HasColumnType("integer");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("People.Persistence.Models.DeceasedStatus", "DeceasedStatus", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<DateTime?>("DeceasedDate")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<bool?>("IsDeceased")
                                .HasColumnType("boolean");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("People.Persistence.Models.Email", "Email", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<string>("Address")
                                .HasColumnType("text");

                            b1.Property<bool?>("IsActive")
                                .HasColumnType("boolean");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("People.Persistence.Models.FullName", "FullName", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<string>("FirstName")
                                .HasColumnType("text");

                            b1.Property<string>("LastName")
                                .HasColumnType("text");

                            b1.Property<string>("MiddleName")
                                .HasColumnType("text");

                            b1.Property<string>("NickName")
                                .HasColumnType("text");

                            b1.Property<string>("Suffix")
                                .HasColumnType("text");

                            b1.Property<string>("Title")
                                .HasColumnType("text");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("BaptismStatus");

                    b.Navigation("BirthDate");

                    b.Navigation("Church");

                    b.Navigation("DeceasedStatus");

                    b.Navigation("Email");

                    b.Navigation("Family");

                    b.Navigation("FullName");
                });

            modelBuilder.Entity("People.Persistence.Models.PhoneNumber", b =>
                {
                    b.HasOne("People.Persistence.Models.Person", null)
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("Churches.Persistence.Models.ChurchGroup", b =>
                {
                    b.Navigation("Churches");
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

            modelBuilder.Entity("People.Persistence.Models.Family", b =>
                {
                    b.Navigation("FamilyMembers");
                });

            modelBuilder.Entity("People.Persistence.Models.Person", b =>
                {
                    b.Navigation("PhoneNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}
