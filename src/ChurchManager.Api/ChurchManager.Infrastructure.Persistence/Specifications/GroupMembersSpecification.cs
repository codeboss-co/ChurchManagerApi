using System.Linq;
using ChurchManager.Domain;
using ChurchManager.Persistence.Models.Groups;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class GroupMembersSpecification : Specification<Group>
    {
        public GroupMembersSpecification(int groupId, RecordStatus recordStatus)
        {
            Criteria = x => x.Id == groupId &&
                x.Members
                    .Any(x => x.RecordStatus == recordStatus);

            IncludeStrings.Add(("Members.Person"));
            IncludeStrings.Add(("Members.GroupRole"));
        }
    }
}
