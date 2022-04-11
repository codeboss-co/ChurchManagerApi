using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Events
{
    public record GroupAttendanceReviewedEvent(int AttendanceId, int GroupId) : IDomainEvent
    {
        public string Feedback { get; set; }
    }
}
