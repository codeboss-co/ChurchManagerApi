using ChurchManager.Application.Features.Groups.Queries.GroupMemberAttendance;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features.Groups.Queries.GroupPerformanceMetrics
{
    public record GroupPerformanceMetricsQuery : IRequest<ApiResponse>
    {
        public int GroupId { get; set; }
        public PeriodType Period { get; set; }
    }

    public class GroupPerformanceMetricsHandler : IRequestHandler<GroupPerformanceMetricsQuery, ApiResponse>
    {
        private readonly IGroupMemberAttendanceDbRepository _dbRepository;
        private readonly IGroupAttendanceDbRepository _groupAttendanceDbRepository;
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IMediator _mediator;

        public GroupPerformanceMetricsHandler(
            IGroupMemberAttendanceDbRepository dbRepository,
            IGroupAttendanceDbRepository groupAttendanceDbRepository,
            IGroupDbRepository groupDbRepository,
            IMediator mediator)
        {
            _dbRepository = dbRepository;
            _groupAttendanceDbRepository = groupAttendanceDbRepository;
            _groupDbRepository = groupDbRepository;
            _mediator = mediator;
        }

        public async Task<ApiResponse> Handle(GroupPerformanceMetricsQuery query, CancellationToken ct)
        {
            var attendanceRecords = (await _mediator.Send(new GroupAttendanceQuery(query.GroupId, query.Period), ct)).Data as GroupMembersAttendanceAnalysisViewModel;

            #region Metrics

            var spec = new GroupPerformanceMetricsSpecification(query.GroupId, query.Period);
            var results = await _dbRepository.ListAsync(spec, ct);

            var firstTimerCount = results.Count(x => x.IsFirstTime.HasValue && x.IsFirstTime.Value);
            var newConvertCount = results.Count(x => x.IsNewConvert.HasValue && x.IsNewConvert.Value);
            var membersCount = (await _groupDbRepository.Queryable("Members").SingleOrDefaultAsync(x => x.Id == query.GroupId, ct))?.Members?.Count ?? 0;

            var metrics = new
            {
                firstTimerCount,
                newConvertCount,
                membersCount
            };
            
            #endregion

            return new ApiResponse( new { metrics, attendanceRecords });
        }
    }
}
