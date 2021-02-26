using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using Convey.CQRS.Queries;
using Domain.Repositories;
using Groups.Persistence.Models;
using Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupDbRepository : CrudDatabaseRepository<Group>, IGroupDbRepository
    {
        public GroupDbRepository(GroupsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Group>> AllPersonsGroups(int personId, CancellationToken ct = default)
        {
            var groups = await Queryable(new AllPersonsGroupsSpecification(personId)).ToListAsync(ct);

            return groups;
        }

        public async Task<PagedResult<Group>> BrowsePersonsGroups(int personId, string search, IPagedQuery query, CancellationToken ct = default)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await BrowseAsync(query, new BrowsePersonsGroupsSpecification(personId, search), ct);
            }

            return await BrowseAsync(query, ct: ct);
        }
    }
}
