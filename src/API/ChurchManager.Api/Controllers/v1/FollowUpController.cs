using ChurchManager.Features.FollowUp.Queries.BrowseFollowUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class FollowUpController : BaseApiController
    {
        #region Browse

        [HttpPost("browse")]
        public async Task<IActionResult> BrowseFollowUps([FromBody] BrowseFollowUpQuery query, CancellationToken token)
        {
            return Ok(await Mediator.Send(query, token));
        }

        #endregion
    }
}
