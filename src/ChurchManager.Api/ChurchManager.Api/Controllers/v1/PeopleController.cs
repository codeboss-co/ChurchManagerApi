using ChurchManager.Features.People.Commands.AddNewFamily;
using ChurchManager.Features.People.Commands.DeletePerson;
using ChurchManager.Features.People.Commands.DeletePhoto;
using ChurchManager.Features.People.Commands.EditPhoto;
using ChurchManager.Features.People.Commands.UpdatePerson;
using ChurchManager.Features.People.Queries.BrowsePeople;
using ChurchManager.Features.People.Queries.FindDuplicates;
using ChurchManager.Features.People.Queries.PeopleAutocomplete;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // v1/people/edit/{personId}/photo/
        [HttpPost("edit/{personId}/photo")]
        public async Task<IActionResult> EditPhoto(int personId, [FromForm(Name = "file")] IFormFile file, CancellationToken token)
        {
            var command = new EditPhotoCommand(personId, file);
            await Mediator.Send(command, token);
            return Accepted();
        }

        // v1/people/edit/{personId}/photo/
        [HttpDelete("edit/{personId}/photo")]
        public async Task<IActionResult> DeletePhoto(int personId, CancellationToken token)
        {
            var command = new DeletePhotoCommand(personId);
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

        [HttpDelete("{personId}")]
        public async Task<IActionResult> DeletePerson(int personId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new DeletePersonCommand(personId), token));
        }
    }
}
