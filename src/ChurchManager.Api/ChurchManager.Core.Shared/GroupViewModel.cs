using System;
using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public int? ChurchId { get; set; }
        public int? ParentGroupId { get; set; }
        public int GroupTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public bool? IsOnline { get; set; }

        public DateTime CreatedDate { get; set; }

        public IEnumerable<GroupViewModel> Groups { get; set; }
    }
}
