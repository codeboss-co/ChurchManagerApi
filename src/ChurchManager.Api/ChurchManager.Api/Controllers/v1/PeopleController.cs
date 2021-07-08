using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Application.Features.People.Commands.AddNewFamily;
using ChurchManager.Application.Features.People.Commands.UpdatePerson;
using ChurchManager.Application.Features.People.Queries.BrowsePeople;
using ChurchManager.Application.Features.People.Queries.FindDuplicates;
using ChurchManager.Application.Features.People.Queries.PeopleAutocomplete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class PeopleController : BaseApiController
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly ICognitoCurrentUser _currentUser;

        public PeopleController(
            ILogger<PeopleController> logger,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        [HttpPost("family/new")]
        public async Task<IActionResult> NewFamily([FromBody] AddNewFamilyCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete([FromQuery] PeopleAutocompleteQuery query, CancellationToken token)
        {
            return Ok(await Mediator.Send(query, token));
        }

        #region Edit Person
        // v1/people/edit/{personId}/connection-info/
        [HttpPost("edit/{personId}/connection-info")]
        public async Task<IActionResult> EditConnectionInfo(int personId, [FromBody] UpdateConnectionInfoCommand command, CancellationToken token)
        {
            command.PersonId = personId;
            await Mediator.Send(command, token);
            return Accepted();
        }

        // v1/people/edit/{personId}/connection-info/
        [HttpPost("edit/{personId}/personal-info")]
        public async Task<IActionResult> EditPersonalInfo(int personId, [FromBody] UpdatePersonalInfoCommand command, CancellationToken token)
        {
            command.PersonId = personId;
            await Mediator.Send(command, token);
            return Accepted();
        }

        // v1/people/edit/{personId}/general-info/
        [HttpPost("edit/{personId}/general-info")]
        public async Task<IActionResult> EditGeneralInfo(int personId, [FromBody] UpdateGeneralInfoCommand command, CancellationToken token)
        {
            command.PersonId = personId;
            await Mediator.Send(command, token);
            return Accepted();
        }

        // v1/people/edit/{personId}/discipleship-info/
        [HttpPost("edit/{personId}/discipleship-info")]
        public async Task<IActionResult> EditDiscipleshipInfo(int personId, [FromBody] UpdateDiscipleshipInfoCommand command, CancellationToken token)
        {
            command.PersonId = personId;
            await Mediator.Send(command, token);
            return Accepted();
        }
        #endregion

        [HttpPost("browse")]
        public async Task<IActionResult> BrowsePeople([FromBody] BrowsePeopleQuery query, CancellationToken token)
        {
            var groups = await Mediator.Send(query, token);
            return Ok(groups);
        }

        [HttpGet("duplicate-check")]
        public async Task<IActionResult> DuplicatePersonCheck([FromQuery] FindPeopleDuplicatesQuery query, CancellationToken token)
        {
            return Ok(await Mediator.Send(query, token));
        }
    }
}
