using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord;

namespace ChurchManager.Application.Features.Groups.Services
{
    public interface IGroupAttendanceAppService
    {
        Task RecordAttendanceAsync(GroupAttendanceRecordCommand command, CancellationToken ct = default);
    }
}