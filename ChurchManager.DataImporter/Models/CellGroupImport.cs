using System;

namespace ChurchManager.DataImporter.Models
{
    public record CellGroupImport
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? IsOnline { get; set; }
        public string ParentGroup { get; set; }
        public string Church { get; set; }
        public DateTimeOffset? StartDate { get; set; }
    }
}
