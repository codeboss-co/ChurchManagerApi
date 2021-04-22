using System;
using System.Collections.Generic;
using ChurchManager.Persistence.Models.Groups;

namespace ChurchManager.Core.Shared
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

    public record GroupAttendanceDetailViewModel : GroupAttendanceViewModel
    {
        public IEnumerable<GroupMemberAttendanceViewModel> Attendees { get; set; }

        // Helpers
        public bool AttendanceReviewed { get; set; }
        public bool AttendanceEntered { get; set; }
        public int DidAttendCount { get; set; }
        public double AttendanceRate { get; set; }
        public AttendanceReview AttendanceReview { get; set; }
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
