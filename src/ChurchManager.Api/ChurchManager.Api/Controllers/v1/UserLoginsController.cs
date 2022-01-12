using ChurchManager.Application.Common;
using ChurchManager.Application.Features.UserLogins.Commands.AddUserLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class UserLoginsController : BaseApiController
    {
        private readonly ILogger<UserLoginsController> _logger;
        private readonly ICognitoCurrentUser _currentUser;

        public UserLoginsController(
            ILogger<UserLoginsController> logger,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        #region Other

        // v1/userlogins/
        [HttpPost]
        public async Task<IActionResult> CreateUserLoginForPerson(AddOrUpdateUserLoginCommand command, CancellationToken token)
        {
            var response = await Mediator.Send(command, token);
            return Ok(response);
        }
        
        #endregion
    }
}
