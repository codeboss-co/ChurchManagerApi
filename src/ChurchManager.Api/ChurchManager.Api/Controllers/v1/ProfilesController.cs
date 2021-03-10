using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Profile.Queries;
using ChurchManager.Application.Features.Profile.Queries.RetrieveProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProfilesController : BaseApiController
    {
        private readonly ILogger<ProfilesController> _logger;

        public ProfilesController(ILogger<ProfilesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("userlogin/{userLoginId}")]
        public async Task<IActionResult> GetUserProfileByUserLogin(string userLoginId, CancellationToken token)
        {
            var person = await Mediator.Send(new ProfileByUserloginIdQuery(userLoginId), token);
            return Ok(person);
        }
    }
}
