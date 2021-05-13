using System;
using System.Collections.Generic;

namespace ChurchManager.Domain.Shared
{
    public record GroupAttendanceViewModel
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

    public record GroupMemberAttendanceViewModel
    {
        public int GroupMemberId { get; set; }
        public bool? DidAttend { get; set; } = true;
        public bool? IsFirstTime { get; set; }
        public bool? IsNewConvert { get; set; }
        public bool? ReceivedHolySpirit { get; set; }
        public string Note { get; set; }
        public virtual GroupMemberViewModel GroupMember { get; set; }
    }
}
