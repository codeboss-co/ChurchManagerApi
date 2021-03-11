using ChurchManager.Persistence.Models.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Infrastructure.Persistence.Specifications
{
    public class ProfileByUserLoginSpecification : Specification<Person>
    {
        public ProfileByUserLoginSpecification(string userLoginId)
        {
            Criteria = x => x.UserLoginId == userLoginId;
        }
    }
}
