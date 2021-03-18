using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Abstractions.Persistence
{
    public interface IGenericRepositoryAsync<T> where T : class, IAggregateRoot<int>
    {
        IChurchManagerDbContext DbContext { get; }
        IQueryable<T> Queryable();
        IQueryable<T> Queryable(ISpecification<T> specification);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetPagedAdvancedResponseAsync(int pageNumber, int pageSize, string orderBy, string fields);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(int id, T sourceItem);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();

        // Paging 
        Task<PagedResult<T>> BrowseAsync<TQuery>(
            TQuery query,
            ISpecification<T> specification = null,
            CancellationToken ct = default) where TQuery : IPagedQuery;
    }
}
