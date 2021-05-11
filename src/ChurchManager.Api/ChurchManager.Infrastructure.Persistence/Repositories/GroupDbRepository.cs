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
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository : GenericRepositoryBase<Group>, IGroupDbRepository
    {
        public GroupDbRepository(DbContext dbContext) : base(dbContext)
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
                    FirstVisitDate = x.FirstVisitDate
                })
                .ToListAsync(ct);

            return members;
        }

        public async Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int maxDepth, CancellationToken ct = default)
        {
            var query = Queryable()
                .AsNoTracking()
                .Include(x => x.GroupType)
                .Where(x => x.ParentGroupId == null) // Exclude children\
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
