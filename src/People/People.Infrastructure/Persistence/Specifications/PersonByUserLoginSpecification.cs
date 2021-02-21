using ChurchManager.Shared.Persistence;
using People.Persistence.Models;

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
