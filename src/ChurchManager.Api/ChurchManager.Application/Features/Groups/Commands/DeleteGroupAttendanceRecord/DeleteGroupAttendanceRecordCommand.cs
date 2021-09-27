using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Commands.DeleteGroupAttendanceRecord
{
    public record DeleteGroupAttendanceRecordCommand(int GroupAttendanceRecordId) : IRequest<ApiResponse>;

    public class DeleteGroupAttendanceRecordHandler : IRequestHandler<DeleteGroupAttendanceRecordCommand, ApiResponse>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;

        public DeleteGroupAttendanceRecordHandler(IGroupAttendanceDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(DeleteGroupAttendanceRecordCommand command, CancellationToken ct)
        {
            var entity = await _dbRepository.GetByIdAsync(command.GroupAttendanceRecordId, ct);

            if (entity is not null)
            {
                await _dbRepository.DeleteAsync(entity, ct);
            }

            return new ApiResponse(entity);
        }
    }
}
