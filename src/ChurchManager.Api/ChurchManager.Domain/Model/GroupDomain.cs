using System.Collections.Generic;
using System.Linq;
using ChurchManager.Persistence.Models.Groups;

namespace ChurchManager.Domain.Model
{
    public class GroupDomain 
    {
        private readonly Group _entity;
        public int GroupId => _entity.Id;
        public int? ChurchId => _entity.ChurchId;
        public int? ParentGroupId => _entity.ParentGroupId;
        public string GroupType => _entity.GroupType.Name;
        public bool TakesAttendance => _entity.GroupType.TakesAttendance;
        public string Name => _entity.Name;
        public string Description => _entity.Description;
        public RecordStatus RecordStatus => _entity.RecordStatus;
        public IReadOnlyList<GroupMemberDomain> Members
            => _entity.Members.Select(x => new GroupMemberDomain(x)).ToList().AsReadOnly();

        public GroupDomain(Group entity) => _entity = entity;
    }

    public class GroupMemberDomain
    {
        private readonly GroupMember _entity;
        public int GroupMemberId => _entity.Id;
        public int GroupId  => _entity.GroupId;
        public RecordStatus RecordStatus => _entity.RecordStatus;
        public bool IsLeader => _entity.GroupRole.IsLeader;
        public PersonDomain Person => new(_entity.Person);

        public GroupMemberDomain(GroupMember entity) => _entity = entity;
    }
}
