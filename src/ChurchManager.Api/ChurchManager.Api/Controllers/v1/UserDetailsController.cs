using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application;
using ChurchManager.Application.Common;
using ChurchManager.Application.Features.Profile.Queries.RetrieveProfile;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class UserDetailsController : BaseApiController
    {
        private readonly ILogger<UserDetailsController> _logger;
        private readonly ICognitoCurrentUser _currentUser;

        public UserDetailsController(
            ILogger<UserDetailsController> logger,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        // v1/userdetails/userlogin/{{userLoginId}}
        [HttpGet("userlogin/{userLoginId}")]
        public async Task<IActionResult> GetUserDetailsSummaryByUserLogin(string userLoginId, CancellationToken token)
        {
            var response = await Mediator.Send(new UserDetailsByUserLoginQuery(userLoginId), token);
            return Ok(response);
        }

        // v1/userdetails/current-user
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUserDetailsSummaryByUserLogin(CancellationToken token)
        {
            var response = await Mediator.Send(new UserDetailsByUserLoginQuery(_currentUser.Id), token);
            return Ok(response);
        }
    }
}
