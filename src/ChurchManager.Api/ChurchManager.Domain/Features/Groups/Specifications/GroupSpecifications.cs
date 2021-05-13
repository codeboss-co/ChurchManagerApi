using Ardalis.Specification;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupWithTypeSpecification : Specification<Group>, ISingleResultSpecification
    {
        public GroupWithTypeSpecification(int groupId)
        {
            Query.AsNoTracking();

            Query.Where(x => x.Id == groupId);

            Query.Include(x => x.GroupType);
        }
    }
}
