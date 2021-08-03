namespace ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord
{
    public record AttendanceBase
    {
        public bool ReceivedHolySpirit { get; set; }
        public bool NewConvert { get; set; }
        public string Note { get; set; }
    }

    public record FirstTimerAttendance : AttendanceBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string AgeClassification { get; set; }
    }

    public record GroupMemberAttendance : AttendanceBase
    {
        public int GroupMemberId { get; set; }
        public string GroupMemberName { get; set; }
        public bool GroupMemberDidAttend { get; set; }
    }
}