using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.People.Events
{
    public record FollowUpAssignedEvent(int PersonId, int AssignedFollowUpPersonId) : IDomainEvent
    {
        public string Type { get; set; }
        public string UserLoginId { get; set; }
    }
}
