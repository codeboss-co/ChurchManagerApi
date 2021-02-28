using ChurchManager.Shared.Persistence;
using People.Persistence.Models;

namespace People.Infrastructure.Persistence.Specifications
{
    public class ProfileByUserLoginSpecification : Specification<Person>
    {
        public ProfileByUserLoginSpecification(string userLoginId)
        {
            Criteria = x => x.UserLoginId == userLoginId;

            IncludeStrings.Add("Family.FamilyMembers");
            Includes.Add(x => x.PhoneNumbers);
        }
    }
}
