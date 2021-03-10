using System.Linq;
using ChurchManager.Domain;
using ChurchManager.Persistence.Models.Groups;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class AllPersonsGroupsSpecification : Specification<Group>
    {
        public AllPersonsGroupsSpecification(int personId, RecordStatus recordStatus)
        {
            Criteria = x =>
                x.Members
                    .Any(x => 
                        x.PersonId == personId && 
                        x.GroupMemberRole.IsLeader &&
                        x.RecordStatus == recordStatus);

            Includes.Add(x => x.GroupType);
            IncludeStrings.Add(("Members.GroupMemberRole"));
        }
    }
}
