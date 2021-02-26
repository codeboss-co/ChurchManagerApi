
using System.Collections.Generic;
using System.Linq;
using Groups.Persistence.Models;
using People.Domain.Model;
using Shared.Kernel;

namespace Domain.Model
{
    public class GroupDomain 
    {
        public int GroupId { get; }
        public int? ChurchId { get; }
        public string GroupType { get; }
        public string Name { get; }
        public string Description { get; }
        public IReadOnlyList<GroupMemberDomain> Members { get; }

        public GroupDomain(Group entity) 
        {
            GroupId = entity.Id;
            ChurchId = entity.ChurchId;
            GroupType = entity.GroupType.Name;
            Name = entity.Name;
            Description = entity.Description;

            Members = entity.Members.Select(x => new GroupMemberDomain(x)).ToList().AsReadOnly();
        }
    }

    public class GroupMemberDomain
    {
        public int GroupId { get; }
        public RecordStatus RecordStatus { get; }
        public bool IsLeader { get; }
        public PersonDomain Person { get; }

        public GroupMemberDomain(GroupMember entity)
        {
            GroupId = entity.GroupId;
            RecordStatus = entity.RecordStatus;
            IsLeader = entity.GroupMemberRole.IsLeader;
            Person = new PersonDomain(entity.Person);
        }
    }
}
