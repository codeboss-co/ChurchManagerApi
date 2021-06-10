using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository : GenericRepositoryBase<Group>, IGroupDbRepository
    {

        public GroupDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
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
                GroupType = new GroupTypeViewModel
                {
                    Id = group.GroupType.Id,
                    Name = group.GroupType.Name,
                    Description = group.GroupType.Description,
                    GroupMemberTerm = group.GroupType.GroupMemberTerm,
                    GroupTerm = group.GroupType.GroupTerm,
                    TakesAttendance = group.GroupType.TakesAttendance,
                    IconCssClass = group.GroupType.IconCssClass,
                },
                CreatedDate = group.CreatedDate,
                ScheduleText = group.Schedule != null ? group.Schedule.ToFriendlyScheduleText(true) : null,
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
