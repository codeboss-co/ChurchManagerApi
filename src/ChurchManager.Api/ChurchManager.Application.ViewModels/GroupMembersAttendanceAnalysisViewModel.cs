using System;
using System.Collections.Generic;

namespace ChurchManager.Application.ViewModels
{
    public record GroupMembersAttendanceAnalysisViewModel
    {
        public IEnumerable<GroupMemberAttendanceAnalysisViewModel> MembersAttendance { get; set; }
    }

    public record GroupMemberAttendanceAnalysisViewModel
    {
        public int GroupMemberId { get; set; }
        public IEnumerable<GroupMemberAttendanceRecord> AttendanceRecords { get; set; }
    }

    public record GroupMemberAttendanceRecord
    {
        public DateTime AttendanceDate { get; set; }
        public bool? DidAttend { get; set; }
    }
}


