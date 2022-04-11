using System.Diagnostics.CodeAnalysis;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.GroupAttendanceRecordSubmissions
{
    public record GroupAttendanceRecordSubmissionsQuery(int ChurchId, PeriodType PeriodType) : IRequest<ApiResponse>;

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

            var allActiveGroups = await _groupDbRepository.Queryable("Members", "Members.GroupRole")
                .AsNoTracking()
                .Where(x =>
                    x.ChurchId == query.ChurchId &&
                    x.GroupTypeId == cellGroupType.Id &&
                    x.RecordStatus == RecordStatus.Active)
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync(ct);

            // Get all group leaders - we need this data so we can contact them if needed
            var dbContext = (ChurchManagerDbContext)_groupTypeRepo.DbContext;
            var groupLeadersSqlQuery = from b in dbContext.GroupMember
                .Where(m =>
                    allActiveGroups.Select(x => x.Id).Contains(m.GroupId) &&
                    m.GroupRole.IsLeader && m.GroupRole.CanEdit && m.GroupRole.CanManageMembers &&
                    m.RecordStatus == RecordStatus.Active)
                select new GroupSubmission(b.GroupId, b.Id, b.Person.FullName.ToString());

            var groupSubmissionsWithLeaderInfo = await groupLeadersSqlQuery.AsNoTracking().ToListAsync(ct);
            var groupLeaderInfoMap = groupSubmissionsWithLeaderInfo
                .Distinct(new GroupSubmissionComparer())
                .ToDictionary(t => t.GroupId, t => t);

            // Get attendance reports for the period
            var spec = new AttendanceReportSubmissionsSpecification(cellGroupType.Id, query.PeriodType);
            var groupIdsWithReports = await _dbRepository.ListAsync<int>(spec, ct);

            // Groups with Reports
            var groupsWithReports = groupIdsWithReports.Join(allActiveGroups, // Join lists
                groupId => groupId, // Join key
                activeGroup => activeGroup.Id, // Join key
                (_, activeGroup) => new
                {
                    activeGroup.Id, activeGroup.Name, Leader = groupLeaderInfoMap.GetOrDefault(activeGroup.Id)
                })
                .ToList(); // Selection

            // Groups without Reports
            var groupIdsWithoutReports = allActiveGroups.Select(x => x.Id)
                .Except(groupsWithReports.Select(x => x.Id));

            var groupsWithoutReports = groupIdsWithoutReports.Join(allActiveGroups, // Join lists
                groupId => groupId, // Join key
                activeGroup => activeGroup.Id, // Join key
                (_, activeGroup) => new
                {
                    activeGroup.Id, activeGroup.Name, Leader = groupLeaderInfoMap.GetOrDefault(activeGroup.Id)
                })
                .ToList(); // Selection

            return new ApiResponse(new
            {
                groupsWithReports = groupsWithReports.OrderBy(x => x.Name),
                groupsWithoutReports = groupsWithoutReports.OrderBy(x => x.Name),
            });
        }
    }

    public class GroupSubmissionComparer : IEqualityComparer<GroupSubmission>
    {
        public bool Equals(GroupSubmission x, GroupSubmission y)
        {
            if (x?.GroupId == y?.GroupId) return true;

            return false;
        }

        public int GetHashCode([DisallowNull] GroupSubmission obj) => obj.GroupId;
    }

    public record GroupSubmission(int GroupId, int PersonId, string PersonName)
    {
    }
}