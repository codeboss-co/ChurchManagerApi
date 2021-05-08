using System.Collections.Generic;
using ChurchManager.Application.Abstractions;
using ChurchManager.Domain.Features.Groups;

namespace ChurchManager.Core.Shared
{
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
}