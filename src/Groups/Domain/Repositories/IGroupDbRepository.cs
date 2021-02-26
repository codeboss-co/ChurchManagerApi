using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using Convey.CQRS.Queries;
using Groups.Persistence.Models;

namespace Domain.Repositories
{
    public interface IGroupDbRepository : ICrudDatabaseRepository<Group>
    {
        Task<IEnumerable<Group>> AllPersonsGroups(int personId, CancellationToken ct = default);
        Task<PagedResult<Group>> BrowsePersonsGroups(int personId, string search, IPagedQuery query, CancellationToken ct = default);
    }
}
