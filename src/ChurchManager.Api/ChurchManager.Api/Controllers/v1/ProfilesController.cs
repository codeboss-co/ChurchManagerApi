using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Profile.Queries.RetrieveProfile;
using ChurchManager.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProfilesController : BaseApiController
    {
        private readonly ILogger<ProfilesController> _logger;
        private readonly ICognitoCurrentUser _currentUser;

        public ProfilesController(
            ILogger<ProfilesController> logger,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        [HttpGet("userlogin/{userLoginId}")]
        public async Task<IActionResult> GetUserProfileByUserLogin(string userLoginId, CancellationToken token)
        {
            var response = await Mediator.Send(new ProfileByUserLoginIdQuery(userLoginId), token);
            return Ok(response);
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserProfileByUserLogin(CancellationToken token)
        {
            var userLoginId = _currentUser.UserLoginId;
            var response = await Mediator.Send(new ProfileByUserLoginIdQuery(userLoginId), token);
            return Ok(response);
        }

        [HttpGet("userdetails/{userLoginId}")]
        public async Task<IActionResult> GetUserDetailsSummaryByUserLogin(string userLoginId, CancellationToken token)
        {
            var response = await Mediator.Send(new UserDetailsByUserLoginQuery(userLoginId), token);
            return Ok(response);
        }
    }
}
