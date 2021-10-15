using System;
using System.Collections.Generic;

namespace ChurchManager.Application.ViewModels
{
    public record GroupMembersAttendanceAnalysisViewModel
    {
        public IEnumerable<GroupMemberAttendanceAnalysisViewModel> MembersAttendance { get; set; }
        public DateTime[] AttendanceDates { get; set; }

        public GroupMembersAttendanceAnalysisViewModel(int capacity)
        {
            MembersAttendance = new List<GroupMemberAttendanceAnalysisViewModel>(capacity);
        }

    }

    public record GroupMemberAttendanceAnalysisViewModel
    {
        public int GroupMemberId { get; set; }
        public bool?[] AttendanceRecords { get; set; }
    }
}


