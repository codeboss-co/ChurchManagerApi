using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Queries;
using Groups.Persistence.Models;
using People.Contracts;

namespace Contracts
{
    public class GroupViewModel : Dictionary<string, object>
    {
        public GroupViewModel(Group entity) : base(6)
        {
            Add("groupId", entity.Id);
            Add("parentGroupId", entity.ParentGroupId);
            Add("groupType", entity.GroupType.Name);
            Add("name", entity.GroupType.Name);
            Add("description", entity.GroupType.Description);
            Add("members", entity.Members.Select(x => new GroupMemberViewModel(x)));
        }

        public GroupViewModel(PagedResult<Group> pagedResult) : base(6)
        {
            Add("currentPage", pagedResult.CurrentPage);
            Add("resultsPerPage", pagedResult.ResultsPerPage);
            Add("totalPages", pagedResult.TotalPages);
            Add("totalResults", pagedResult.TotalResults);

            Add("items", pagedResult.Items.Select(x => new GroupViewModel(x)));
        }
    }

    public class GroupMemberViewModel : Dictionary<string, object>
    {
        public GroupMemberViewModel(GroupMember entity) : base(5)
        {
            Add("groupId", entity.GroupId);
            Add("recordStatus", entity.RecordStatus);
            Add("isLeader", entity.GroupMemberRole.IsLeader);

            Add("person", new PersonViewModel(entity.Person));
        }
    }
}
