using ChurchManager.Features.Groups.Commands.DeleteGroupAttendanceRecord;
using ChurchManager.Features.Groups.Queries.GroupAttendanceRecordSubmissions;
using ChurchManager.Features.Groups.Queries.GroupMemberAttendance;
using ChurchManager.Features.Groups.Queries.Reports.AttendanceReportGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
