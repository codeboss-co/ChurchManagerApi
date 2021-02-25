using System;
using System.Threading.Tasks;
using Codeboss.Types;
using People.Domain.Model;

namespace Shared.Kernel.Security
{
    public interface ICognitoCurrentUser : ICurrentUser
    {
        /// <summary>
        /// Lazily loads the Current User
        /// </summary>
        Lazy<Task<PersonDomain>> CurrentPerson { get; }
    }
}
