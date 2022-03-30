using Ardalis.Specification;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupTypeForGroupSpecification : Specification<Group, GroupType>, ISingleResultSpecification
    {
        public GroupTypeForGroupSpecification(int groupId)
        {
            Query.AsNoTracking();

            Query.Where(group => group.Id == groupId);

            Query.Select(x => x.GroupType);
        }
    }
}
