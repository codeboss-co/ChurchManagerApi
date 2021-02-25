using System.Collections.Generic;
using System.Linq;
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
    }

    public class GroupMemberViewModel : Dictionary<string, object>
    {
        public GroupMemberViewModel(GroupMember entity) : base(5)
        {
            Add("groupId", entity.GroupId);
            Add("groupMemberStatus", entity.GroupMemberStatus);
            Add("isLeader", entity.GroupMemberRole.IsLeader);
            Add("isActive", entity.InactiveDateTime == null);

            Add("person", new PersonViewModel(entity.Person));
        }
    }
}
