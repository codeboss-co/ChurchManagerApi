using Codeboss.Types;

namespace ChurchManager.Infrastructure.Abstractions.Security
{
    public interface ICognitoCurrentUser<TDomainModel> : ICurrentUser
    {
        /// <summary>
        /// Lazily loads the Current User
        /// </summary>
        Lazy<Task<TDomainModel>> CurrentPerson { get; }
    }
}
