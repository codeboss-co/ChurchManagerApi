using System;
using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public class GroupAttendanceViewModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool? DidNotOccur { get; set; }
        public int? AttendanceCount { get; set; }
        public int? FirstTimerCount { get; set; }
        public int? NewConvertCount { get; set; }
        public int? ReceivedHolySpiritCount { get; set; }
        public string Notes { get; set; }
        public IEnumerable<string> PhotoUrls { get; set; }
    }
}
