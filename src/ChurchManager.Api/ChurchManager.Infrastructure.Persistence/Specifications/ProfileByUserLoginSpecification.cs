using ChurchManager.Domain.Model.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class ProfileByUserLoginSpecification : Specification<Person>
    {
        public ProfileByUserLoginSpecification(string userLoginId)
        {
            Criteria = x => x.UserLoginId == userLoginId;

            Includes.Add(x => x.Church);
            Includes.Add(x => x.PhoneNumbers);
            IncludeStrings.Add("Family.FamilyMembers");
        }
    }

    public class ProfileByPersonSpecification : Specification<Person>
    {
        public ProfileByPersonSpecification(int personId, bool condensed = false)
        {
            Criteria = x => x.Id == personId;

            Includes.Add(x => x.Church);
            Includes.Add(x => x.PhoneNumbers);

            if (!condensed)
            {
                IncludeStrings.Add("Family.FamilyMembers");
            }
        }
    }
}
