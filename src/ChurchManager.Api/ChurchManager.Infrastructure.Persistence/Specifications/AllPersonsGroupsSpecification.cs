using System.Linq;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
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
                        x.GroupRole.IsLeader &&
                        x.RecordStatus == recordStatus);

            Includes.Add(x => x.GroupType);
            IncludeStrings.Add(("Members.GroupRole"));
        }
    }
}
