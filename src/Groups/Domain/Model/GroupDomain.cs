
using System.Collections.Generic;
using System.Linq;
using Groups.Persistence.Models;
using People.Domain.Model;

namespace Domain.Model
{
    public class GroupDomain : Dictionary<string, object>
    {
        public GroupDomain(Group entity) : base(5)
        {
            Add("groupId", entity.Id);
            Add("groupType", entity.GroupType.Name);
            Add("name", entity.GroupType.Name);
            Add("description", entity.GroupType.Description);
            Add("members", entity.Members.Select(x => new GroupMemberDomain(x)));
        }
    }

    public class GroupMemberDomain : Dictionary<string, object>
    {
        public GroupMemberDomain(GroupMember entity) : base(5)
        {
            Add("groupId", entity.GroupId);
            Add("groupMemberStatus", entity.GroupMemberStatus);
            Add("isLeader", entity.GroupMemberRole.IsLeader);
            Add("isActive", entity.InactiveDateTime == null);

            Add("person", new PersonDomain(entity.Person));
        }
    }
}
