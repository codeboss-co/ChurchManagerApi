using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions.Security;

namespace ChurchManager.Domain.Common
{
    // Marker for easy reference
    public interface ICognitoCurrentUser : ICognitoCurrentUser<PersonDomain>
    {
        public string Username { get; }
        public int PersonId { get; }
    }
}
