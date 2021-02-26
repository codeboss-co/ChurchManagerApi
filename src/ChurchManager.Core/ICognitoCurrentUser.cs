using People.Domain.Model;
using Shared.Kernel.Security;

namespace ChurchManager.Core
{
    // Marker for easy reference
    public interface ICognitoCurrentUser: ICognitoCurrentUser<PersonDomain>
    {
    }
}