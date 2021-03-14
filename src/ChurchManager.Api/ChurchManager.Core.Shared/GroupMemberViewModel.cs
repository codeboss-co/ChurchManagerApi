namespace ChurchManager.Core.Shared
{
    public record GroupMemberViewModel
    {
        public int GroupMemberId { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }
    }
}
