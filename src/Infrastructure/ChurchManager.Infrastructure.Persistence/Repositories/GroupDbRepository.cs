using AutoMapper;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository : GenericRepositoryBase<Group>, IGroupDbRepository
    {
        private readonly IMapper _mapper;

        public GroupDbRepository(ChurchManagerDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, RecordStatus status, CancellationToken ct = default)
        {
            var spec = new GroupMembersSpecification(groupId, status);
            var queryable = ApplySpecification(spec);

            var members = await queryable
                .SelectMany(x => x.Members)
                .Select(x => new GroupMemberViewModel
                {
                    PersonId = x.PersonId,
                    GroupId = x.GroupId,
                    GroupMemberId = x.Id,
                    FirstName = x.Person.FullName.FirstName,
                    MiddleName = x.Person.FullName.MiddleName,
                    LastName = x.Person.FullName.LastName,
                    Gender = x.Person.Gender,
                    PhotoUrl = x.Person.PhotoUrl,
                    GroupMemberRoleId = x.GroupRoleId,
                    GroupMemberRole = x.GroupRole.Name,
                    IsLeader = x.GroupRole.IsLeader,
                    FirstVisitDate = x.FirstVisitDate,
                    RecordStatus = x.RecordStatus
                })
                .ToListAsync(ct);

            return members;
        }

        /// <summary>
        /// Gets groups and children that have a specific parent
        /// </summary>
        public async Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int? parentGroupId = null, int maxDepth = 10, CancellationToken ct = default)
        {
            var query = Queryable()
                .AsNoTracking()
                .Include(x => x.GroupType)
                .Include(x => x.Schedule)
                .Where(x => x.ParentGroupId == parentGroupId) // null will start at the root of the tree
                .Select(GroupProjection(maxDepth))
                ;

            return await query.ToListAsync(ct);
        }

        /// <summary>
        /// Gets group and children of that group
        /// </summary>
        public async Task<IEnumerable<GroupViewModel>> GroupWithChildrenAsync(int groupId, int maxDepth = 10, CancellationToken ct = default)
        {
            var query = Queryable()
                    .AsNoTracking()
                    .Include(x => x.GroupType)
                    .Include(x => x.Schedule)
                    .Where(x => x.Id == groupId) 
                    .Select(GroupProjection(maxDepth))
                ;

            return await query.ToListAsync(ct);
        }

        /// <summary>
        /// Group statistics by group type
        /// </summary>
        public async Task<(int totalGroupsCount, int activeGroupsCount, int inActiveGroupsCount, int onlineGroupsCount, int openedGroupsCount, int closedGroupsCount)> GroupStatisticsAsync(int groupTypeId, DateTime? startDate = null, CancellationToken ct = default)
        {
            var groups = await Queryable()
                .AsNoTracking()
                .Where(x => x.GroupTypeId == groupTypeId)
                .Select(x => new { x.Id, x.RecordStatus, x.IsOnline, x.StartDate, x.InactiveDateTime })
                .ToListAsync(ct);

            var totalCellsCount = groups.Count;
            var activeCellsCount = groups.Count(x => x.RecordStatus == RecordStatus.Active);
            var inActiveCellsCount = totalCellsCount - activeCellsCount;
            var onlineCellsCount = groups.Count(x => x.IsOnline.HasValue && x.IsOnline.Value);

            if (startDate is null)
            {
                startDate = DateTime.UtcNow.AddMonths(-6);
            }

            var openedGroupsCount = groups
                .Count(x => x.StartDate >= startDate);

            var closedGroupsCount = groups
                .Count(
                    x => x.InactiveDateTime != null &&
                         x.InactiveDateTime >= startDate &&
                         x.RecordStatus != RecordStatus.Active);

            return (totalCellsCount, activeCellsCount, inActiveCellsCount, onlineCellsCount, openedGroupsCount, closedGroupsCount);
        }

        /// <summary>
        /// https://michaelceber.medium.com/implementing-a-recursive-projection-query-in-c-and-entity-framework-core-240945122be6
        /// </summary>
        private Expression<Func<Group, GroupViewModel>> GroupProjection(int maxDepth, int currentDepth = 0)
        {
            currentDepth++;

            Expression<Func<Group, GroupViewModel>> result = group => new GroupViewModel
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Address = group.Address,
                StartDate = group.StartDate,
                ChurchId = group.ChurchId,
                ParentGroupId = group.ParentGroupId,
                ParentGroupName = group.ParentGroup.Name,
                IsOnline = group.IsOnline,
                GroupType = _mapper.Map<GroupTypeViewModel>(group.GroupType),
                CreatedDate = group.CreatedDate,
                Schedule = _mapper.Map<ScheduleViewModel>(group.Schedule),
                Level = currentDepth,
                Groups = currentDepth == maxDepth
                    ? new List<GroupViewModel>(0) // Reached maximum depth so stop
                    : group.Groups.AsQueryable()
                        .Include(x => x.GroupType)
                        .Select(GroupProjection(maxDepth, currentDepth))
                        .ToList()
            };

            return result;
        }
    }
}
