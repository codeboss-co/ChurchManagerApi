using ChurchManager.Shared.Persistence;
using People.Infrastructure.Persistence.Model;

namespace People.Infrastructure.Persistence.Specifications
{
    public class PersonByUserLoginSpecification : Specification<Person>
    {
        public PersonByUserLoginSpecification(string userLoginId)
        {
            Criteria = x => x.UserLoginId == userLoginId;
        }
    }
}
