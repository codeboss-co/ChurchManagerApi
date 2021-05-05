using System.Linq;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Groups;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupMemberDbRepository: GenericRepositoryAsync<GroupMember>, IGroupMemberDbRepository
    {
        public GroupMemberDbRepository(IChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Returns a queryable collection of <see cref="GroupMember">GroupMembers</see> who are members of a specific group.
        /// </summary>
        public IQueryable<GroupMember> GetByGroupId(int groupId)
        {
            return Queryable("Person,GroupRole")
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
    }
}
