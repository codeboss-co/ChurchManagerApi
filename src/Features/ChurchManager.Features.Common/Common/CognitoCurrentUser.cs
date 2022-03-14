using System.Security.Claims;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using Codeboss.Types;

namespace ChurchManager.Features.Common.Common
{
    public class CognitoCurrentUser : ICognitoCurrentUser
    {
        internal const string ClaimTypeUsername = ClaimTypes.Name;
        private readonly IProfileService _applicationService;
        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public CognitoCurrentUser(ICurrentPrincipalAccessor principalAccessor, IProfileService applicationService)
        {
            _principalAccessor = principalAccessor;
            _applicationService = applicationService;
        }

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);
        public string Id => _principalAccessor.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Username => _principalAccessor.Principal.FindFirstValue(ClaimTypeUsername);
        public int PersonId => CurrentPerson.Value.GetAwaiter().GetResult().PersonId;

        /// <summary>
        ///     Loads the Current User using the
        /// </summary>
        public Lazy<Task<PersonViewModel>> CurrentPerson =>
            new(async () => await _applicationService.ProfileByUserLoginId(Id));
    }
}