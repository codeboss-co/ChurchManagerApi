using ChurchManager.Features.Groups.Commands.GroupAttendanceRecord;

namespace ChurchManager.Features.Groups.Services
{
    public interface IGroupAttendanceAppService
    {
        Task RecordAttendanceAsync(GroupAttendanceRecordCommand command, CancellationToken ct = default);
    }
}