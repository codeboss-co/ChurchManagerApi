using System;
using System.Threading.Tasks;
using Codeboss.Types;

namespace Shared.Kernel.Security
{
    public interface ICognitoCurrentUser<TDomainModel> : ICurrentUser
    {
        /// <summary>
        /// Lazily loads the Current User
        /// </summary>
        Lazy<Task<TDomainModel>> CurrentPerson { get; }
    }
}
