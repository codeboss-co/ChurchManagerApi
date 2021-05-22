using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken token)
        {
            var result = await Mediator.Send(command, token);

            if (!result.IsAuthenticated)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
