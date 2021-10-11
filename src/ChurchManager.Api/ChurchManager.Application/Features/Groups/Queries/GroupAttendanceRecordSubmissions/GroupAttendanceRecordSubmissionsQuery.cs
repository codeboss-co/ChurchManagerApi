using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Application.Features.Groups.Queries.GroupAttendanceRecordSubmissions
{
    public record GroupAttendanceRecordSubmissionsQuery : IRequest<ApiResponse>
    {
        public int ChurchId { get; set; }
        public PeriodType PeriodType { get; set; }
    }

    public class GroupAttendanceRecordSubmissionsHandler : IRequestHandler<GroupAttendanceRecordSubmissionsQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IGroupAttendanceDbRepository _dbRepository;
        private readonly IGenericDbRepository<GroupType> _groupTypeRepo;

        public GroupAttendanceRecordSubmissionsHandler(
            IGroupDbRepository groupDbRepository,
            IGroupAttendanceDbRepository dbRepository,
            IGenericDbRepository<GroupType> groupTypeRepo)
        {
            _groupDbRepository = groupDbRepository;
            _dbRepository = dbRepository;
            _groupTypeRepo = groupTypeRepo;
        }


        public async Task<ApiResponse> Handle(GroupAttendanceRecordSubmissionsQuery query, CancellationToken ct)
        {
            // TODO: Pass in group type id from so we can support all groups
            var cellGroupType = await _groupTypeRepo.Queryable().FirstOrDefaultAsync(x => x.Name == "Cell", ct);

            var allActiveGroups = await _groupDbRepository.Queryable("Members", "Members.GroupRole", "Members.Person")
                .AsNoTracking()
                .Where(x => 
                    x.ChurchId == query.ChurchId &&
                    x.GroupTypeId == cellGroupType.Id &&
                    x.RecordStatus == RecordStatus.Active)
                .Select(x => 
                    new { x.Id, x.Name,
                        Leader = x.Members.FirstOrDefault(m => 
                             m.GroupRole.IsLeader &&
                             m.RecordStatus == RecordStatus.Active) })
                .ToListAsync(ct);

            /*var allActiveGroups = allActiveGroupsWithLeaders.Select(x =>
            {
                var leaderName = x.Leader?.Person?.FullName?.ToString();

                return new { x.Id, x.Name, Leader = leaderName };
            });*/

            var spec = new AttendanceReportSubmissionsSpecification(cellGroupType.Id, query.PeriodType);
            var groupIdsWithReports = await _dbRepository.ListAsync<int>(spec, ct);

            var groupsWithReports = groupIdsWithReports.Join(allActiveGroups,  // Join lists
                groupId => groupId, // Join key
                activeGroup => activeGroup.Id, // Join key
                (_, activeGroup) => new { activeGroup.Id, activeGroup.Name, activeGroup.Leader });  // Selection

            var groupIdsWithoutReports = allActiveGroups.Select(x => x.Id).Except(groupsWithReports.Select(x => x.Id));

            var groupsWithoutReports = groupIdsWithoutReports.Join(allActiveGroups,  // Join lists
                groupId => groupId, // Join key
                activeGroup => activeGroup.Id, // Join key
                (_, activeGroup) => new { activeGroup.Id, activeGroup.Name, activeGroup.Leader });  // Selection

            return new ApiResponse(new { groupsWithReports, groupsWithoutReports });
        }
    }
}