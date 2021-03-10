using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions.Security;

namespace ChurchManager.Domain
{
    // Marker for easy reference
    public interface ICognitoCurrentUser : ICognitoCurrentUser<PersonDomain>
    {
    }
}
