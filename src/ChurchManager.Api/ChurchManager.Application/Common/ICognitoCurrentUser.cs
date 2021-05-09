using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Infrastructure.Abstractions.Security;

namespace ChurchManager.Application.Common
{
    // Marker for easy reference
    public interface ICognitoCurrentUser : ICognitoCurrentUser<PersonViewModel>
    {
        public string Username { get; }
        public int PersonId { get; }
    }
}
