using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features.Groups.Queries.GroupAttendanceRecordSubmissions
{
    public record GroupAttendanceRecordSubmissionsQuery : IRequest<ApiResponse>
    {
        public int ChurchId { get; set; }
        public int GroupTypeId { get; set; }
        public PeriodType PeriodType { get; set; }
    }

    public class GroupAttendanceRecordSubmissionsHandler : IRequestHandler<GroupAttendanceRecordSubmissionsQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IGroupAttendanceDbRepository _dbRepository;

        public GroupAttendanceRecordSubmissionsHandler(
            IGroupDbRepository groupDbRepository,
            IGroupAttendanceDbRepository dbRepository)
        {
            _groupDbRepository = groupDbRepository;
            _dbRepository = dbRepository;
        }


        public async Task<ApiResponse> Handle(GroupAttendanceRecordSubmissionsQuery query, CancellationToken ct)
        {
            var allActiveGroups = await _groupDbRepository.Queryable()
                .AsNoTracking()
                .Where(x => 
                    x.ChurchId == query.ChurchId &&
                    x.RecordStatus == RecordStatus.Active)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync(ct);

            var spec = new AttendanceReportSubmissionsSpecification(query.GroupTypeId, query.PeriodType);
            var groupIdsWithReports = await _dbRepository.ListAsync<int>(spec, ct);

            var groupsWithReports = groupIdsWithReports.Join(allActiveGroups,  // Join lists
                groupId => groupId, // Join key
                activeGroup => activeGroup.Id, // Join key
                (_, activeGroup) => new { activeGroup.Id, activeGroup.Name });  // Selection

            var groupIdsWithoutReports = allActiveGroups.Select(x => x.Id).Except(groupsWithReports.Select(x => x.Id));

            var groupsWithoutReports = groupIdsWithoutReports.Join(allActiveGroups,  // Join lists
                groupId => groupId, // Join key
                activeGroup => activeGroup.Id, // Join key
                (_, activeGroup) => new { activeGroup.Id, activeGroup.Name });  // Selection

            return new ApiResponse(new { groupsWithReports, groupsWithoutReports });
        }
    }
}