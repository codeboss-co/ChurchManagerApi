using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ChurchesController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public ChurchesController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> AllChurches(CancellationToken token)
        {
            var groups = await Mediator.Send(new ChurchesQuery(), token);
            return Ok(groups);
        }
    }
}
