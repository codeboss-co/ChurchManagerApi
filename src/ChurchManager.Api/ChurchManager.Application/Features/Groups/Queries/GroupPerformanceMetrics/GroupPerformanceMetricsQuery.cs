using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        private readonly IGroupDbRepository _groupDbRepository;

        public GroupPerformanceMetricsHandler(
            IGroupMemberAttendanceDbRepository dbRepository,
            IGroupDbRepository groupDbRepository)
        {
            _dbRepository = dbRepository;
            _groupDbRepository = groupDbRepository;
        }

        public async Task<ApiResponse> Handle(GroupPerformanceMetricsQuery query, CancellationToken ct)
        {
            var spec = new GroupPerformanceMetricsSpecification(query.GroupId, query.Period);
            var results = await _dbRepository.ListAsync(spec, ct);

            var firstTimerCount = results.Count(x => x.IsFirstTime.HasValue && x.IsFirstTime.Value);
            var newConvertCount = results.Count(x => x.IsNewConvert.HasValue && x.IsNewConvert.Value);
            var membersCount = (await _groupDbRepository.Queryable("Members").SingleAsync(x => x.Id == query.GroupId)).Members?.Count ?? 0;

            var metrics = new
            {
                firstTimerCount,
                newConvertCount,
                membersCount
            };

            return new ApiResponse(metrics);
        }
    }
}
