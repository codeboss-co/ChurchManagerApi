using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Services;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord
{
    public record GroupAttendanceRecordCommand : IRequest
    {
        public int GroupId { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public bool? DidNotOccur { get; set; }
        public IEnumerable<GroupMemberAttendance> Members { get; set; }
        public IEnumerable<FirstTimerAttendance> FirstTimers { get; set; }
        public string Note { get; set; }
    }

    public class GroupAttendanceHandler : IRequestHandler<GroupAttendanceRecordCommand>
    {
        private readonly IGroupAttendanceAppService _appService;

        public GroupAttendanceHandler(IGroupAttendanceAppService appService)
        {
            _appService = appService;
        }

        public async Task<Unit> Handle(GroupAttendanceRecordCommand command, CancellationToken ct)
        {
            await _appService.RecordAttendanceAsync(command, ct);

            return new Unit();
        }
    }
}
