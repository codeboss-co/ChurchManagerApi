using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class DashboardController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IChurchAttendanceDbRepository _attendanceDbRepository;

        public DashboardController(ICognitoCurrentUser currentUser, IChurchAttendanceDbRepository attendanceDbRepository)
        {
            _currentUser = currentUser;
            _attendanceDbRepository = attendanceDbRepository;
        }

        [HttpGet("church-attendance")]
        public async Task<IActionResult> ChurchAttendance([FromQuery] DateTime from, DateTime to, CancellationToken token)
        {
            var attendances = await _attendanceDbRepository.DashboardChurchAttendanceAsync(from, to);
            return Ok(attendances);
        }

        [HttpGet("church-attendance-breakdown")]
        public async Task<IActionResult> ChurchAttendanceBreakdown([FromQuery] DateTime from, DateTime to, CancellationToken token)
        {
            var breakdown = await _attendanceDbRepository.DashboardChurchAttendanceBreakdownAsync(from, to);
            return Ok(breakdown);
        }
    }
}
