using System.Linq;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The data access/service class for <see cref="GroupTypeRole"/> entity object types.
    /// </summary>
    public class GroupTypeRoleDbRepository : GenericRepositoryAsync<GroupTypeRole>, IGroupTypeRoleDbRepository
    {
        public GroupTypeRoleDbRepository(IChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets the by group type identifier.
        /// </summary>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <returns></returns>
        public IQueryable<GroupTypeRole> GetByGroupTypeId(int groupTypeId)
        {
            return Queryable()
                .Where(r => r.GroupTypeId == groupTypeId)
                .OrderBy(r => r.Name);
        }
    }
}
