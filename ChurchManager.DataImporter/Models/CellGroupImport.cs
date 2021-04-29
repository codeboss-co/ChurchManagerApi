using System;
using System.Collections.Generic;
using System.Linq;
using ChurchManager.Persistence.Models.Churches;
using ChurchManager.Persistence.Models.Groups;

namespace ChurchManager.DataImporter.Models
{
    public record CellGroupImport
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? IsOnline { get; set; }
        public string ParentGroupName { get; set; }
        public string Church { get; set; }
        public DateTimeOffset? StartDate { get; set; }

        public static Group ToEntity(CellGroupImport import, IList<Church> churches, IList<Group> groups = null) => new()
        {
            Name = import.Name,
            Description = import.Description,
            Address = import.Address,
            ChurchId = churches.FirstOrDefault(x => x.Name == import.Church)?.Id,
            IsOnline = import.IsOnline,
            StartDate = import.StartDate,
            GroupTypeId = 1,
            ParentGroupId = groups?.FirstOrDefault(x => x.Name == import.ParentGroupName)?.Id
        };
    }
}
