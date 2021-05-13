using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class OnlineUserSpecification : Specification<OnlineUser>, ISingleResultSpecification
    {
        public OnlineUserSpecification(string userLoginId)
        {
            Query.Include(x => x.Person);

            Query.Where(p => p.Person.UserLoginId == userLoginId);
        }
    }

    public class OnlineUsersSpecification : Specification<OnlineUser, OnlineUserViewModel>
    {
        public OnlineUsersSpecification(IDateTimeProvider dateTime)
        {
            Query.AsNoTracking();
            Query.Include(x => x.Person);

            Query.Select(x => new OnlineUserViewModel
            {
                Id = x.Person.UserLoginId,
                Name = $"{x.Person.FullName.FirstName} {x.Person.FullName.LastName}",
                Avatar = x.Person.PhotoUrl,
                Status = x.Status,
                Unread = 2,
                LastOnline = dateTime.ConvertFromUtc(x.LastOnlineDateTime.UtcDateTime)
            });
        }
    }
}
