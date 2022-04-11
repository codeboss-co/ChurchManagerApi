using Ardalis.Specification;
using ChurchManager.Domain.Common;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class UserDetailsSpecification : Specification<Person, UserDetails>, ISingleResultSpecification
    {
        public UserDetailsSpecification(string userLoginId)
        {
            Query.AsNoTracking();

            Query.Where(x => x.UserLoginId == userLoginId);

            Query.Select(x => new UserDetails
            {
                UserLoginId = x.UserLoginId,
                FirstName = x.FullName.FirstName,
                LastName = x.FullName.LastName,
                Email = x.Email.Address,
                PhotoUrl = x.PhotoUrl
            });
        }
    }
}
