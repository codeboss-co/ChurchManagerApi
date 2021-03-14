using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups;
using ChurchManager.Application.Features.Groups.Queries.GroupMembers;
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
        public async Task<IActionResult> BrowseGroups(int personId, [FromBody] BrowsePersonsGroupsQuery query, CancellationToken token)
        {
            query.PersonId = personId; // Reset to the person we are searching for in the URL
            return Ok(await Mediator.Send(query, token));
        }

        [HttpPost("browse/current-user")]
        public async Task<IActionResult> BrowseCurrentUserGroups([FromBody] BrowsePersonsGroupsQuery query, CancellationToken token)
        {
            query.PersonId = _currentUser.PersonId; // Reset to current person
            return Ok(await Mediator.Send(query, token));
        }

        [HttpGet("{groupId}/members")]
        public async Task<IActionResult> GetGroupMembers(int groupId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GroupMembersQuery(groupId), token));
        }
    }
}
