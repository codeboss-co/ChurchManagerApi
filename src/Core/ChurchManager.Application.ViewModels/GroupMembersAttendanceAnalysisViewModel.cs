using System;
using System.Collections.Generic;

namespace ChurchManager.Application.ViewModels
{
    public record GroupMembersAttendanceAnalysisViewModel
    {
        public IEnumerable<GroupMemberAttendanceAnalysisViewModel> MembersAttendance { get; set; }
        public DateTime[] AttendanceDates { get; set; }
        public double AvgAttendanceRate { get; set; }
        public GroupMembersAttendanceAnalysisViewModel(int capacity)
        {
            MembersAttendance = new List<GroupMemberAttendanceAnalysisViewModel>(capacity);
        }

    }

    public record GroupMemberAttendanceAnalysisViewModel
    {
        public int GroupMemberId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public bool?[] AttendanceRecords { get; set; }
    }
}


