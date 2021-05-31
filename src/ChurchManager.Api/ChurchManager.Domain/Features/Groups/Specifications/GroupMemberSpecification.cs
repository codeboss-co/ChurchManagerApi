using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupMemberSpecification : Specification<GroupMember, GroupMemberEditViewModel>, ISingleResultSpecification
    {
        public GroupMemberSpecification(int groupMemberId)
        {
            Query.AsNoTracking();

            Query.Where(x => x.Id == groupMemberId);

            Query.Include(x => x.Person);

            Query.Select(x => new GroupMemberEditViewModel
            {
                GroupMemberId = x.Id,
                Person = new AutocompleteResult(x.PersonId, $"{x.Person.FullName.FirstName} {x.Person.FullName.LastName}"),
                CommunicationPreference = x.CommunicationPreference,
                GroupRole = x.GroupRoleId,
                RecordStatus = x.RecordStatus,
                FirstVisitDate = x.FirstVisitDate
            });
        }
    }
}
