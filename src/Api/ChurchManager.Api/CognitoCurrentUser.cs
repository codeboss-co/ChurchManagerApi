using System.Security.Claims;
using Codeboss.Types;

namespace ChurchManager.Api
{
    public class CognitoCurrentUser : ICurrentUser
    {
        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public CognitoCurrentUser(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);

        public string Id => _principalAccessor.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
