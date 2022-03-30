using ChurchManager.Features.Missions.Commands.AddMission;
using ChurchManager.Features.Missions.Queries.BrowseMissions;
using ChurchManager.Features.Missions.Queries.GetMission;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> AddMission([FromBody] AddMissionCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpGet("{missionId}")]
        public async Task<IActionResult> GetMission(int missionId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GetMissionQuery(missionId), token));
        }
    }
}
