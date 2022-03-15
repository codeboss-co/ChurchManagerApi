using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.BrowseGroupAttendance
{
    public record AttendanceRecordQuery(int AttendanceRecordId) : IRequest<GroupAttendanceDetailViewModel>
    {
    }

    public class AttendanceRecordHandler : IRequestHandler<AttendanceRecordQuery, GroupAttendanceDetailViewModel>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public AttendanceRecordHandler(IGroupAttendanceDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<GroupAttendanceDetailViewModel> Handle(AttendanceRecordQuery query, CancellationToken ct)
        {
            var spec = new GroupAttendanceSpecification(query.AttendanceRecordId);
            var entity = await _dbRepository.GetBySpecAsync(spec, ct);

            var vm = _mapper.Map<GroupAttendanceDetailViewModel>(entity);
            return vm;
        }
    }
}