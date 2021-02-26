using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ChurchManager.Core;
using Codeboss.Types;
using People.Application.Services;
using People.Domain.Model;

namespace ChurchManager.Api
{
    public class CognitoCurrentUser : ICognitoCurrentUser
    {
        private readonly ICurrentPrincipalAccessor _principalAccessor;
        private readonly IPersonApplicationService _applicationService;

        public CognitoCurrentUser(ICurrentPrincipalAccessor principalAccessor, IPersonApplicationService applicationService)
        {
            _principalAccessor = principalAccessor;
            _applicationService = applicationService;
        }

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);

        public string Id => _principalAccessor.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Loads the Current User using the
        /// </summary>
        public Lazy<Task<PersonDomain>> CurrentPerson => new(async () => await _applicationService.PersonByUserLoginId(Id));
    }
}
