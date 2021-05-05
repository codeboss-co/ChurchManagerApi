﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChurchManager.DataImporter.Models;
using ChurchManager.Domain.Model.Churches;
using ChurchManager.Domain.Model.Discipleship;
using ChurchManager.Domain.Model.Groups;
using ChurchManager.Domain.Model.People;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ChurchManager.DataImporter
{
    class Program
    {
        private const int ChurchesSheet = 0;
        private const int CellGroupsSheet = 1;
        private const int FamiliesSheet = 2;
        private const int PeopleSheet = 3;

        public static readonly CalendarSerializer CalendarSerializer = new();

        static ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        static DbContextOptions<ChurchManagerDbContext> _options = new DbContextOptionsBuilder<ChurchManagerDbContext>()
            .UseNpgsql("Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;")
            //.UseLoggerFactory(_loggerFactory) //Optional, this logs SQL generated by EF Core to the Console
            .Options;

        static void Main(string[] args)
        {
            string path = args != null && args.Any() ? args[0] : "./churchmanager_db_data_import.xlsx";
            var data = Process(path);
        }

        public static IEnumerable<IEnumerable<object>> Process(string path)
        {
            using var file = new FileStream(path, FileMode.Open, FileAccess.Read);
            return ImportExcel(file, true);
        }

        public static IEnumerable<IEnumerable<object>> ImportExcel(Stream stream, bool skipFistRow)
        {
            ISheet sheet;

            XSSFWorkbook workbook = new XSSFWorkbook(stream); //This will read 2007 Excel format  
            sheet = workbook.GetSheetAt(0); //get first sheet from workbook   

            var churches = ChurchesImported(workbook.GetSheetAt(ChurchesSheet), skipFistRow);
            var groups = CellGroupsImported(workbook.GetSheetAt(CellGroupsSheet), skipFistRow);
            var families = FamiliesImported(workbook.GetSheetAt(FamiliesSheet), skipFistRow);
            var people = PeopleImported(workbook.GetSheetAt(PeopleSheet), skipFistRow);

            IList<Church> churchDbList = new List<Church>(churches.Count);

            using (var dbContext = new ChurchManagerDbContext(_options))
            {
                DiscipleshipStepDefinition foundationSchoolStepDefinition;
                DiscipleshipStepDefinition baptismClassStepDefinition;
                if(!dbContext.DiscipleshipProgram.Any())
                {
                    foundationSchoolStepDefinition = new DiscipleshipStepDefinition
                    {
                        Name = "Foundation School",
                        Description = "Basics of our Faith and Doctrine",
                        IconCssClass = "heroicons-solid:academic-cap",
                        Order = 0
                    };
                    baptismClassStepDefinition = new DiscipleshipStepDefinition
                    {
                        Name = "Baptism Class",
                        Description = "Understanding Baptism",
                        AllowMultiple = false,
                        Order = 1
                    };

                    var newConvertsDiscipleshipProgram = new DiscipleshipProgram
                    {
                        Name = "New Christians Program",
                        Description = "Discipleship for New Converts",
                        Category = "New Christians",
                        StepDefinitions = new List<DiscipleshipStepDefinition>(2) { foundationSchoolStepDefinition, baptismClassStepDefinition }
                    };

                    dbContext.Add(newConvertsDiscipleshipProgram);
                    dbContext.SaveChanges();
                }
                else
                {
                    foundationSchoolStepDefinition = dbContext.DiscipleshipStepDefinition.First(x => x.Name == "Foundation School");
                    baptismClassStepDefinition = dbContext.DiscipleshipStepDefinition.First(x => x.Name == "Baptism Class");
                }

                // Cell Group Type
                var  cellGroupType = new GroupType { Name = "Cell", Description = "Cell Ministry", IconCssClass = "heroicons_outline:share" };
                // Cell Group Roles
                var cellLeaderRole = new GroupTypeRole
                {
                    Name = "Leader", Description = "Leader of the Group", IsLeader = true, 
                    CanView = true, CanEdit = true, CanManageMembers = true,
                    GroupType = cellGroupType
                };
                var cellAssistantRole = new GroupTypeRole { Name = "Assistant", Description = "Assistant Leader", GroupType = cellGroupType };
                var cellMemberRole = new GroupTypeRole { Name = "Member", Description = "Group Member", GroupType = cellGroupType };

                if(!dbContext.GroupType.Any())
                {
                    dbContext.Add(cellGroupType);
                    dbContext.GroupTypeRole.Add(cellLeaderRole);
                    dbContext.GroupTypeRole.Add(cellAssistantRole);
                    dbContext.GroupTypeRole.Add(cellMemberRole);
                    dbContext.SaveChanges();
                }

                if(!dbContext.Church.Any())
                {
                    dbContext.AddRange(churches);
                    var inserted = dbContext.SaveChanges();
                    Console.WriteLine($"Churches added: {inserted}");
                }

                if(!dbContext.Family.Any())
                {
                    dbContext.AddRange(families);
                    var inserted = dbContext.SaveChanges();
                    Console.WriteLine($"Families added: {inserted}");
                }

                if(!dbContext.Group.Any())
                {
                    Console.WriteLine();
                    Console.WriteLine("*** Groups ***");

                    churchDbList = dbContext.Church.ToList();

                    // First insert all root parent groups i.e. they have no parents
                    var parentGroups = groups
                        .Where(import => import.ParentGroupName.IsNullOrEmpty())
                        .Select(x => CellGroupImport.ToEntity(x, churchDbList))
                        .ToList();

                    dbContext.AddRange(parentGroups);
                    var inserted = dbContext.SaveChanges();
                    Console.WriteLine($"\t > Root Groups added: {inserted}");

                    var children = new List<Group>(0);
                    while (parentGroups.Any())
                    {
                        // Get direct children of the parents
                        children = groups
                            .Where(child =>
                            {
                                var parentGroupsNames = parentGroups.Select(x => x.Name);
                                return parentGroupsNames.Contains(child.ParentGroupName);
                            })
                            .Select(x => CellGroupImport.ToEntity(x, churchDbList, dbContext.Group.ToList()))
                            .ToList();

                        if (children.Any())
                        {
                            dbContext.AddRange(children);
                            inserted = dbContext.SaveChanges();
                            Console.WriteLine($"\t\t > Children added: {inserted}");
                        }

                        // Make the children the new parents and start again
                        parentGroups = children;
                        Console.WriteLine($"\t\t > Processing new parents");
                    }
                }

                if(!dbContext.Person.Any())
                {
                    churchDbList = dbContext.Church.ToList();

                    // Get all the cell groups in the database
                    var cellGroups = dbContext.Group
                        .Include(x => x.GroupType)
                        .Where(x => x.GroupType.Name == "Cell")
                        .ToList();

                    var groupRoles = dbContext.GroupTypeRole.ToList();
                    var familyDbList = dbContext.Family.ToList();
                    
                    // We need this map later for discipleship imports
                    var personMap = new Dictionary<PersonImport, Person>(people.Count);

                    // First we add members of groups
                    var groupMembers = people
                        .Where(x => !string.IsNullOrEmpty(x.CellGroupName))
                        .Select(x =>
                        {
                            var group = cellGroups.FirstOrDefault(g => g.Name == x.CellGroupName) ?? throw new ArgumentNullException(nameof(x.CellGroupName));
                            var groupRole = groupRoles.FirstOrDefault(r => r.Name == x.CellGroupRole) ?? throw new ArgumentNullException(nameof(x.CellGroupRole));
                            var church = churchDbList.FirstOrDefault(c => c.Name == x.ChurchName) ?? throw new ArgumentNullException(nameof(x.ChurchName));
                            var family = familyDbList.FirstOrDefault(c => c.Name == x.FamilyName) ?? throw new ArgumentNullException(nameof(x.FamilyName));

                            List<PhoneNumber> phoneNumbers = null;
                            if (!string.IsNullOrEmpty(x.PhoneNumber))
                            {
                                phoneNumbers = new List<PhoneNumber> {new() {CountryCode = "+27", Number = x.PhoneNumber}};
                            }

                            var person = new Person
                            {
                                AgeClassification = x.AgeClassification,
                                BaptismStatus = x.Baptism,
                                AnniversaryDate = x.AnniversaryDate,
                                BirthDate = x.BirthDate,
                                ChurchId = church.Id,
                                CommunicationPreference = x.CommunicationPreference,
                                ConnectionStatus = x.ConnectionStatus,
                                Email = new Email {IsActive = true, Address = x.Email},
                                FamilyId = family.Id,
                                FirstVisitDate = x.FirstVisitDate,
                                Gender = x.Gender,
                                MaritalStatus = x.MaritalStatus,
                                Occupation = x.Occupation,
                                FullName = x.FullName,
                                PhoneNumbers = phoneNumbers,
                                PhotoUrl = x.PhotoUrl,
                                Source = x.Source,
                                ReceivedHolySpirit = x.ReceivedHolySpirit,
                                UserLoginId = x.UserLoginId
                            };

                            // We need to keep this mapping
                            personMap.Add(x, person);

                            return new GroupMember
                            {
                                CommunicationPreference = x.CommunicationPreference,
                                FirstVisitDate = x.FirstVisitDate,
                                GroupId = group.Id,
                                GroupRoleId = groupRole.Id,
                                Person = person
                            };
                        }).ToList();

                    dbContext.GroupMember.AddRange(groupMembers);
                    dbContext.SaveChanges();
                    Console.WriteLine($"\t > Group Members added: {groupMembers.Count}");

                    // Add Discipleship program and steps
                    foreach (var importAndPerson in personMap)
                    {
                        AddDiscipleshipStepsToPerson(dbContext, foundationSchoolStepDefinition,
                            baptismClassStepDefinition, importAndPerson.Key, importAndPerson.Value);
                    }
                    Console.WriteLine($"\t > Discipleship Steps added.");
                    dbContext.SaveChanges();

                }
            }
            

            var rowIndex = skipFistRow ? 1 : 0;
            var firstRow = sheet.GetRow(rowIndex); //Get Header Row
            var cellCount = firstRow.LastCellNum;
            var data = new List<List<string>>();

            for(int i = rowIndex; i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var row = sheet.GetRow(i);

                if(row == null)
                    continue;
                if(row.Cells.All(d => d.CellType == CellType.Blank))
                    continue;

                var rowData = new List<string>();

                for(int j = row.FirstCellNum; j < cellCount; j++)
                {
                    var value = row.GetCell(j)?.ToString() ?? string.Empty;

                    rowData.Add(value);
                }

                data.Add(rowData);
            }

            return data;
        }


        private static IList<Church> ChurchesImported(ISheet sheet, bool skipFistRow)
        {
            var rowIndex = skipFistRow ? 1 : 0;

            var churches = new List<Church>(sheet.LastRowNum + 1);

            for(int i = rowIndex; i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var row = sheet.GetRow(i);

                if(row == null)
                    continue;
                if(row.Cells.All(d => d.CellType == CellType.Blank))
                    continue;
                
                string name = row.GetCell(0)?.StringCellValue; // Name = Column 0
                string description = row.GetCell(1)?.StringCellValue; // Description = Column 1
                string address = row.GetCell(2)?.StringCellValue; // Address = Column 2
                string shortcode = row.GetCell(3)?.StringCellValue; 
                string phoneNumber = PhoneNumber.CleanNumber(row.GetCell(4)?.StringCellValue);

                var church = new Church
                {
                    Name = name,
                    Description = description,
                    Address = address,
                    ShortCode = shortcode,
                    PhoneNumber = phoneNumber
                };

                churches.Add(church);
            }

            return churches;
        }

        private static IList<CellGroupImport> CellGroupsImported(ISheet sheet, bool skipFistRow)
        {
            var rowIndex = skipFistRow ? 1 : 0;

            var groups = new List<CellGroupImport>(sheet.LastRowNum + 1);

            for(int i = rowIndex; i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var row = sheet.GetRow(i);

                if(row == null)
                    continue;
                if(row.Cells.All(d => d.CellType == CellType.Blank))
                    continue;

                string name = row.GetCell(0)?.StringCellValue; // Name = Column 0
                string description = row.GetCell(1)?.StringCellValue; // Description = Column 1
                string address = row.GetCell(2)?.StringCellValue; // Address = Column 2
                bool? isOnline = row.GetCell(3)?.StringCellValue != null && row.GetCell(3)?.StringCellValue == "Yes";
                string parentGroup = row.GetCell(4)?.StringCellValue;
                string church = row.GetCell(5)?.StringCellValue;
                DateTime? startDate = row.GetCell(6)?.DateCellValue;
                string meetingDay = row.GetCell(7)?.StringCellValue;
                string meetingTime = row.GetCell(8)?.StringCellValue;

                var group = new CellGroupImport
                {
                    Name = name,
                    Description = description,
                    Address = address,
                    IsOnline = isOnline,
                    ParentGroupName = parentGroup,
                    Church = church,
                    StartDate = startDate,
                    MeetingDay = meetingDay,
                    MeetingTime = meetingTime
                };

                groups.Add(group);
            }

            return groups;
        }

        private static IList<Family> FamiliesImported(ISheet sheet, bool skipFistRow)
        {
            var rowIndex = skipFistRow ? 1 : 0;

            var data = new List<Family>(sheet.LastRowNum + 1);

            for(int i = rowIndex; i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var row = sheet.GetRow(i);

                if(row == null)
                    continue;
                if(row.Cells.All(d => d.CellType == CellType.Blank))
                    continue;

                string name = row.GetCell(0)?.StringCellValue; // Name = Column 0
                string street = row.GetCell(1)?.StringCellValue; // Description = Column 1
                string city = row.GetCell(2)?.StringCellValue; // Address = Column 2
                string country = row.GetCell(3)?.StringCellValue;
                string province = row.GetCell(4)?.StringCellValue;
                string postalCode = row.GetCell(5)?.NumericCellValue.ToString() ?? string.Empty;
                string language = row.GetCell(6)?.StringCellValue;

                var item = new Family
                {
                    Name = name,
                    Address = new Address
                    {
                        Street = street,
                        City = city,
                        Country = country,
                        Province = province,
                        PostalCode = postalCode
                    },
                    Language = language
                };

                data.Add(item);
            }

            return data;
        }

        private static IList<PersonImport> PeopleImported(ISheet sheet, bool skipFistRow)
        {
            var rowIndex = skipFistRow ? 1 : 0;

            var data = new List<PersonImport>(sheet.LastRowNum + 1);

            for(int i = rowIndex; i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var row = sheet.GetRow(i);

                if(row == null)
                    continue;
                if(row.Cells.All(d => d.CellType == CellType.Blank))
                    continue;

                string name = row.GetCell(0)?.StringCellValue; // Name = Column 0
                string title = row.GetCell(1)?.StringCellValue; 
                string firstName = row.GetCell(2)?.StringCellValue; 
                string nickName = row.GetCell(3)?.StringCellValue;
                string middleName = row.GetCell(4)?.StringCellValue;
                string lastName = row.GetCell(5)?.StringCellValue;
                string suffix = row.GetCell(6)?.StringCellValue;
                string connectionStatus = row.GetCell(7)?.StringCellValue;
                string ageClassification = row.GetCell(8)?.StringCellValue;
                string gender = row.GetCell(9)?.StringCellValue;
                int? birthDay = (int?)row.GetCell(10)?.NumericCellValue;
                int? birthMonth = (int?)row.GetCell(11)?.NumericCellValue;
                int? birthYear = (int?)row.GetCell(12)?.NumericCellValue;
                string source = row.GetCell(13)?.StringCellValue;
                DateTime? firstVisitDate = row.GetCell(14)?.DateCellValue;
                bool? isBaptised = row.GetCell(15)?.StringCellValue != null && row.GetCell(15)?.StringCellValue == "Yes";
                DateTime? baptismDate = row.GetCell(16)?.DateCellValue;
                bool? foundationSchoolComplete = row.GetCell(17)?.StringCellValue != null && row.GetCell(17)?.StringCellValue == "Yes";
                DateTime? foundationSchoolDate = row.GetCell(18)?.DateCellValue;
                bool? holySpirit = row.GetCell(19)?.StringCellValue != null && row.GetCell(19)?.StringCellValue == "Yes";
                string maritalStatus = row.GetCell(20)?.StringCellValue;
                DateTime? anniversary = row.GetCell(21)?.DateCellValue;
                string phone = PhoneNumber.CleanNumber(row.GetCell(22)?.StringCellValue);
                string email = row.GetCell(23)?.StringCellValue;
                string communicationPreference = row.GetCell(24)?.StringCellValue;
                string photoUrl = row.GetCell(25)?.StringCellValue;
                string occupation = row.GetCell(26)?.StringCellValue;
                string church = row.GetCell(27)?.StringCellValue;
                string cellGroup = row.GetCell(28)?.StringCellValue;
                string cellGroupRole = row.GetCell(29)?.StringCellValue;
                string userLoginId = row.GetCell(30)?.StringCellValue;

                var item = new PersonImport
                {
                    FamilyName = name,
                    FullName = new FullName
                    {
                        Title = title,
                        FirstName = firstName,
                        NickName = nickName,
                        MiddleName = middleName,
                        LastName = lastName,
                        Suffix = suffix
                    },
                    ConnectionStatus = connectionStatus,
                    AgeClassification = ageClassification,

                    Gender = gender,
                    BirthDate = new BirthDate
                    {
                        BirthDay = birthDay,
                        BirthMonth = birthMonth,
                        BirthYear = birthYear,
                    },
                    Source = source,
                    FirstVisitDate = firstVisitDate,
                    Baptism = new Baptism
                    {
                        IsBaptised = isBaptised,
                        BaptismDate = baptismDate
                    },
                    FoundationSchool = new FoundationSchool
                    {
                        IsComplete = foundationSchoolComplete,
                        CompletionDate = foundationSchoolDate
                    },
                    ReceivedHolySpirit = holySpirit,
                    MaritalStatus = maritalStatus,
                    AnniversaryDate = anniversary,
                    PhoneNumber = phone,
                    Email = email,
                    CommunicationPreference = communicationPreference,
                    PhotoUrl = photoUrl,
                    Occupation = occupation,
                    ChurchName = church,
                    CellGroupName = cellGroup,
                    CellGroupRole = cellGroupRole
                };

                data.Add(item);
            }

            return data;
        }

        private static void AddDiscipleshipStepsToPerson(
            ChurchManagerDbContext dbContext,
            DiscipleshipStepDefinition foundationSchoolStepDef,
            DiscipleshipStepDefinition baptismClassStepDef,
            PersonImport import,
            Person person
        )
        {
            var foundationSchool = import.FoundationSchool;
            var baptism = import.Baptism;

            var personFoundationSchoolStep = new DiscipleshipStep
            {
                Definition = foundationSchoolStepDef,
                CompletionDate = foundationSchool.CompletionDate,
                Status = foundationSchool.IsComplete.HasValue && foundationSchool.IsComplete.Value ? "Completed" : "Not Completed",
                Person = person
            };
            var baptismClassStep = new DiscipleshipStep
            {
                Definition = baptismClassStepDef,
                StartDateTime = baptism.BaptismDate,
                Status = baptism.IsBaptised.HasValue && baptism.IsBaptised.Value ? "Completed" : "Not Completed",
                Person = person
            };

            dbContext.Add(personFoundationSchoolStep);
            dbContext.Add(baptismClassStep);
        }
    }
}
