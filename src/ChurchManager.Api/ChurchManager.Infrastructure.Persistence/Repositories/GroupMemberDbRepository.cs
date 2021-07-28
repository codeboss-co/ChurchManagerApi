using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupMemberDbRepository: GenericRepositoryBase<GroupMember>, IGroupMemberDbRepository
    {
        public GroupMemberDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Returns a queryable collection of <see cref="GroupMember">GroupMembers</see> who are members of a specific group.
        /// </summary>
        public IQueryable<GroupMember> GetByGroupId(int groupId)
        {
            return Queryable("Person", "GroupRole")
                .Where(t => t.GroupId == groupId)
                .OrderBy(g => g.GroupRole.Name);
        }

        /// <summary>
        /// Gets the active leaders of the group
        /// </summary>
        public IQueryable<GroupMember> GetLeaders(int groupId)
        {
            return GetByGroupId(groupId)
                .AsNoTracking()
                .Where(t =>
                    t.RecordStatus == RecordStatus.Active &&
                    t.GroupRole.IsLeader);
        }

        public async Task<(int peopleCount, int leadersCount)> PeopleAndLeadersInGroupsAsync(int groupTypeId)
        {
            var query = Queryable("GroupRole", "Group")
                .Where(x => x.Group.GroupTypeId == groupTypeId);

            var peopleCount = await query
                .CountAsync(m => m.RecordStatus == RecordStatus.Active && !m.GroupRole.IsLeader);

            // Extra strict requirements as Cell Assistants are also marked leaders
            var leadersCount = await query
                .CountAsync(m => m.RecordStatus == RecordStatus.Active && m.GroupRole.IsLeader && m.GroupRole.CanEdit && m.GroupRole.CanManageMembers);

            return (peopleCount, leadersCount);
        }
    }
}
