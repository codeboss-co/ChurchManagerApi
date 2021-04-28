﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChurchManager.DataImporter.Models;
using ChurchManager.Persistence.Models.Churches;
using ChurchManager.Persistence.Models.People;
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
                string phoneNumber = row.GetCell(4)?.StringCellValue;

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

                var group = new CellGroupImport
                {
                    Name = name,
                    Description = description,
                    Address = address,
                    IsOnline = isOnline,
                    ParentGroup = parentGroup,
                    Church = church,
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
                string phone = row.GetCell(22)?.StringCellValue;
                string email = row.GetCell(23)?.StringCellValue;
                string communicationPreference = row.GetCell(24)?.StringCellValue;
                string photoUrl = row.GetCell(25)?.StringCellValue;
                string occupation = row.GetCell(26)?.StringCellValue;
                string church = row.GetCell(27)?.StringCellValue;
                string cellGroup = row.GetCell(28)?.StringCellValue;
                string cellGroupRole = row.GetCell(29)?.StringCellValue;

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
    }
}
