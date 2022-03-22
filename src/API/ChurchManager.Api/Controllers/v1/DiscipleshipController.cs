using ChurchManager.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class DiscipleshipController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public DiscipleshipController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet("programs")]
        public async Task<IActionResult> GetDiscipleshipPrograms(CancellationToken token)
        {
            return Ok(await Mediator.Send(new DiscipleshipProgramsQuery(), token));
        }


        [HttpGet("programs/{id}")]
        public async Task<IActionResult> GetDiscipleshipProgramById(int id, CancellationToken token)
        {
            return Ok(await Mediator.Send(new DiscipleshipProgramsQuery{DiscipleshipProgramId = id}, token));
        }

        [HttpPost("person/programs")]
        public async Task<IActionResult> GetDiscipleshipForPerson([FromBody] DiscipleshipForPersonQuery query, CancellationToken token)
        {
            query.PersonId ??= _currentUser.PersonId;
            return Ok(await Mediator.Send(query, token));
        }

        [HttpGet("types/{typeId}/definitions")]
        public async Task<IActionResult> GetDiscipleshipDefinitionSteps(int typeId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new DiscipleshipDefinitionStepsQuery(typeId), token));
        }

        [HttpPost("steps/{definitionId}/people/browse")]
        public async Task<IActionResult> BrowsePeopleInDiscipleshipStep(int definitionId, [FromBody] BrowseDiscipleshipStepParticipantsQuery query, CancellationToken token)
        {
            query.DiscipleshipStepDefinitionId = definitionId;
            return Ok(await Mediator.Send(query, token));
        }

        [HttpPost("person/step")]
        public async Task<IActionResult> GetDiscipleshipStepInfoForPerson([FromBody] DiscipleshipStepInfoForPersonQuery query, CancellationToken token)
        {
            query.PersonId ??= _currentUser.PersonId;
            return Ok(await Mediator.Send(query, token));
        }
    }
}
