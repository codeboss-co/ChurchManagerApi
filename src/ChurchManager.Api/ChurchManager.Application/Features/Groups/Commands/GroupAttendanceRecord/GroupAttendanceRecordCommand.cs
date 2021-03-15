using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord
{
    public record GroupAttendanceRecordCommand : IRequest
    {
        public DateTime MeetingDate { get; set; }
        public IEnumerable<GroupMemberAttendance> Members { get; set; }
        public IEnumerable<FirstTimerAttendance> FirstTimers { get; set; }
    }

    public class GroupAttendanceHandler : IRequestHandler<GroupAttendanceRecordCommand>
    {
        public Task<Unit> Handle(GroupAttendanceRecordCommand command, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
