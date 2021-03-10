using System.Collections.Generic;

namespace ChurchManager.Application.Features.Groups.Queries.GroupsForPerson
{
    public record GroupSummaryViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentGroupId { get; set; }
        public string GroupType { get; set; }
        public string RecordStatus { get; set; }
        public int MembersCount { get; set; }
    }

    public class GroupSummariesViewModel : List<GroupSummaryViewModel>
    {
        public GroupSummariesViewModel(IEnumerable<GroupSummaryViewModel> vms)
        {
            AddRange(vms);
        }
    }
}
