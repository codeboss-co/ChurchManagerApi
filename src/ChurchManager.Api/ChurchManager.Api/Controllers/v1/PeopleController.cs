﻿using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.People.Commands.AddNewFamily;
using ChurchManager.Domain;
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
        public async Task<IActionResult> PostGroupAttendanceRecord([FromBody] AddNewFamilyCommand command, CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }
    }
}
