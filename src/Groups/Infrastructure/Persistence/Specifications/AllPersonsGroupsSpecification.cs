using System.Linq;
using ChurchManager.Shared.Persistence;
using Groups.Persistence.Models;

namespace Infrastructure.Persistence.Specifications
{
    public class AllPersonsGroupsSpecification : Specification<Group>
    {
        public AllPersonsGroupsSpecification(int personId)
        {
            Criteria = x => x.Members.Any(x => x.PersonId == personId
                                               && x.GroupMemberRole.IsLeader);

            Includes.Add(x => x.GroupType);
            Includes.Add(x => x.Members);
            IncludeStrings.Add(("Members.GroupMemberRole"));
        }
    }
}
