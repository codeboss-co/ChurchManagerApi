using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class DiscipleshipController : BaseApiController
    {

        [HttpGet("types")]
        public async Task<IActionResult> GetDiscipleshipTypes(CancellationToken token)
        {
            return Ok(await Mediator.Send(new DiscipleshipTypesQuery(), token));
        }

        [HttpGet("types/{typeId}/definitions")]
        public async Task<IActionResult> GetDiscipleshipDefinitionSteps(int typeId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new DiscipleshipDefinitionStepsQuery(typeId), token));
        }

        [HttpGet("person/{personId}/programs")]
        public async Task<IActionResult> GetDiscipleshipStepsForPerson(int personId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new DiscipleshipForPersonQuery(personId), token));
        }
    }
}
