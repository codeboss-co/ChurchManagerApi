using System;

namespace ChurchManager.Domain.Shared
{
    public record GroupMemberViewModel
    {
        public int GroupId { get; set; }
        public int GroupMemberId { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }
        public int GroupMemberRoleId { get; set; }
        public string GroupMemberRole { get; set; }
        public bool IsLeader { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public string RecordStatus { get; set; }

    }

    public record GroupMemberEditViewModel
    {
        public int GroupMemberId { get; set; }
        public AutocompleteResult Person { get; set; }
        public int GroupRole { get; set; }
        public string CommunicationPreference { get; set; }
        public string RecordStatus { get; set; }
        public DateTime? FirstVisitDate { get; set; }
    }
}
