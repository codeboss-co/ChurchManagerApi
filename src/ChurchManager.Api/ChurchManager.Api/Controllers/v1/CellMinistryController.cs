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
    public class CellMinistryController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public CellMinistryController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> AllCellGroups(CancellationToken token)
        {
            var groups = await Mediator.Send(new ChurchesQuery(), token);
            return Ok(groups);
        }
    }
}
