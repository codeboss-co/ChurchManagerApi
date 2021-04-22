using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Queries.BrowseGroupAttendance
{
    public record AttendanceRecordQuery(int AttendanceRecordId) : IRequest<GroupAttendanceViewModel>
    {
    }

    public class AttendanceRecordHandler : IRequestHandler<AttendanceRecordQuery, GroupAttendanceViewModel>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public AttendanceRecordHandler(IGroupAttendanceDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<GroupAttendanceViewModel> Handle(AttendanceRecordQuery query, CancellationToken ct)
        {
            var entity = await _dbRepository
                .Queryable(new GroupAttendanceSpecification(query.AttendanceRecordId))
                .FirstOrDefaultAsync(ct);

            return _mapper.Map<GroupAttendanceViewModel>(entity);
        }
    }
}
