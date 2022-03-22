using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Common;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class PersonsGroupsSpecification : Specification<Group>
    {
        public PersonsGroupsSpecification(int personId, RecordStatus recordStatus)
        {
            Query.AsNoTracking();

            Query.Where(x =>
                x.Members
                    .Any(x =>
                        x.PersonId == personId &&
                        x.GroupRole.IsLeader &&
                        x.RecordStatus == recordStatus));

            Query.Include(x => x.GroupType);
            Query.Include(x => x.Members)
                .ThenInclude(x => x.GroupRole);
        }  
    }
}
