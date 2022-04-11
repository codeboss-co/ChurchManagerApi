using Ardalis.Specification;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class ProfileByUserLoginSpecification : Specification<Person>, ISingleResultSpecification
    {
        public ProfileByUserLoginSpecification(string userLoginId)
        {
            Query
                .Where(x => x.UserLoginId == userLoginId)

                .Include(x => x.Church)
                .Include(x => x.PhoneNumbers)
                .Include(x => x.Family).ThenInclude(f => f.FamilyMembers);
        }
    }

    public class ProfileByPersonSpecification : Specification<Person>, ISingleResultSpecification
    {
        public ProfileByPersonSpecification(int personId, bool condensed = false)
        {
            Query
                .Where(x => x.Id == personId)

                .Include(x => x.Church)
                .Include(x => x.PhoneNumbers);

            if(!condensed)
            {
                Query.Include(x => x.Family).ThenInclude(f => f.FamilyMembers);
            }
        }
    }
}
