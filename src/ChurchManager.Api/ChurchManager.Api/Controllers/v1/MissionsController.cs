using ChurchManager.Application.Common;
using ChurchManager.Application.Features.Missions.Queries.BrowseMissions;
using ChurchManager.Application.Features.Missions.Queries.GetMission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class MissionsController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public MissionsController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpPost("browse")]
        public async Task<IActionResult> Browse([FromBody] BrowseMissionsQuery query, CancellationToken token)
        {
            var groups = await Mediator.Send(query, token);
            return Ok(groups);
        }

        [HttpGet("{missionId}")]
        public async Task<IActionResult> GetMission(int missionId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GetMissionQuery(missionId), token));
        }
    }
}
