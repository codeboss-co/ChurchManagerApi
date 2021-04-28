using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChurchManager.DataImporter.Models;
using ChurchManager.Persistence.Models.Churches;
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


            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

            var churches = ChurchesImported(hssfwb.GetSheetAt(ChurchesSheet), skipFistRow);
            var groups = CellGroupsImported(hssfwb.GetSheetAt(CellGroupsSheet), skipFistRow);

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
                bool? isOnline = row.GetCell(3)?.BooleanCellValue;
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
    }
}
