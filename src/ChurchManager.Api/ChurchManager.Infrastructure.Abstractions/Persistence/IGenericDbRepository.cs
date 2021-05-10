using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Codeboss.Types;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Abstractions.Persistence
{
    public interface IGenericDbRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot<int>
    {
        DbContext DbContext { get; }
        IQueryable<T> Queryable(params string[] includes);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<PagedResult<T>> BrowseAsync(IPagedQuery query, ISpecification<T> specification, CancellationToken ct = default);
    }
}
