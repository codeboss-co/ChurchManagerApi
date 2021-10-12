using ChurchManager.Application.Features.Groups.Commands.DeleteGroupAttendanceRecord;
using ChurchManager.Application.Features.Groups.Queries.Reports.AttendanceReportGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Queries.GroupAttendanceRecordSubmissions;
using ChurchManager.Application.Features.Groups.Queries.GroupMemberAttendance;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class GroupAttendanceController : BaseApiController
    {
        [HttpDelete("{groupAttendanceId}")]
        public async Task<IActionResult> DeleteAttendanceRecord(int groupAttendanceId, CancellationToken token)
        {
            var data = await Mediator.Send(new DeleteGroupAttendanceRecordCommand(groupAttendanceId), token);
            return Ok(data);
        }


        [HttpPost("attendance-report-grid")]
        public async Task<IActionResult> AttendanceReportGrid([FromBody] AttendanceReportGridQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }

        [HttpPost("report-submissions")]
        public async Task<IActionResult> GroupAttendanceRecordSubmissions([FromBody] GroupAttendanceRecordSubmissionsQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }

        [HttpGet("members-attendance")]
        public async Task<IActionResult> GroupMembersAttendance([FromQuery] GroupMembersAttendanceQuery query, CancellationToken token)
        {
            var data = await Mediator.Send(query, token);
            return Ok(data);
        }
    }
}
