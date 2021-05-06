using ChurchManager.Domain.Features.Groups;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class GroupRolesForGroupSpecification : Specification<Group>
    {
        public GroupRolesForGroupSpecification(int groupId)
        {
            Criteria = group => group.Id == groupId;

             IncludeStrings.Add("Members.GroupRole");
        }
    }
}
