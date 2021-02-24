
using System.Collections.Generic;
using System.Linq;
using Groups.Persistence.Models;

namespace Domain.Model
{
    public class GroupDomain
    {
        public int GroupId { get; }
        public string GroupType { get;}
        public string Name { get; }
        public string Description { get;  }
        public IEnumerable<GroupMemberDomain> Members { get; }

        public GroupDomain(Group entity)
        {
            GroupId = entity.Id;
            GroupType = entity.GroupType.Name;
            Name = entity.Name;
            Description = entity.Description;
            Members = entity.Members.Select(x => new GroupMemberDomain(x));
        }
    }

    public class GroupMemberDomain
    {
        public int PersonId { get; }
        public int GroupId { get; }
        public string GroupMemberStatus { get; }
        public bool IsLeader { get;}
        public bool IsActive { get;}

        public GroupMemberDomain(GroupMember entity)
        {
            PersonId = entity.PersonId;
            GroupId = entity.GroupId;
            GroupMemberStatus = entity.GroupMemberStatus;
            IsLeader = entity.GroupMemberRole.IsLeader;
            IsActive = entity.InactiveDateTime == null;
        }
    }
}
