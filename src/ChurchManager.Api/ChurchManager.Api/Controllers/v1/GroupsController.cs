using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Api.Requests;
using ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class GroupsController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public GroupsController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet("person/{personId}")]
        public async Task<IActionResult> GetAllPersonsGroups(int personId, CancellationToken token)
        {
            var groups = await Mediator.Send(new GroupsForPersonQuery(personId), token);
            return Ok(groups);
        }

        [HttpPost("browse/person/{personId}")]
        //[Authorize]
        public async Task<IActionResult> BrowseGroups(int personId, [FromBody] SearchTermRequest request, CancellationToken token)
        {
            return Ok(await Mediator.Send(new BrowsePersonsGroupsQuery(request.SearchTerm, personId), token));
        }
    }
}
