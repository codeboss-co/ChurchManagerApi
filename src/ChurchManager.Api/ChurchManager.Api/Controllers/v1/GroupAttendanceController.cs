using ChurchManager.Application.Features.Groups.Commands.DeleteGroupAttendanceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

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
    }
}
