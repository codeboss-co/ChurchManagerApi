using ChurchManager.Application.Features.Groups.Commands.DeleteGroupAttendanceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Queries.Reports.AttendanceReportGrid;

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
    }
}
