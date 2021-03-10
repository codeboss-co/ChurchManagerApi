using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Extensions;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using ConveyPaging = Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class, IAggregateRoot<int>
    {
        #region Fields

        internal DbSet<T> ObjectSet;

        #endregion

        #region Properties

        public DbContext DbContext { get; }

        #endregion

        #region Constructors

        protected GenericRepositoryAsync(DbContext dbContext)
        {
            DbContext = dbContext;
            ObjectSet = DbContext.Set<T>();
        }
        #endregion

        public IQueryable<T> Queryable() => ObjectSet;

        public virtual IQueryable<T> Queryable(ISpecification<T> specification)
        {
            if(specification is null)
            {
                return ObjectSet;
            }

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = specification.Includes
                .Aggregate(ObjectSet.AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = specification.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            if(specification.Criteria != null)
            {
                return secondaryResult.Where(specification.Criteria);
            }

            return secondaryResult;
        }

        public virtual async Task<T> GetByIdAsync(int id) => await ObjectSet.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await ObjectSet.ToListAsync();

        public async Task<IEnumerable<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await ObjectSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAdvancedResponseAsync(int pageNumber, int pageSize, string orderBy, string fields)
        {
            return await ObjectSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select<T>("new(" + fields + ")")
                .OrderBy(orderBy)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await ObjectSet.AddAsync(entity);
            return entity;
        }
        
        public async Task UpdateAsync(int id, T sourceItem)
        {
            var targetItem = await ObjectSet.FindAsync(id);
            if(targetItem != null)
            {
                DbContext.Entry(targetItem).CurrentValues.SetValues(sourceItem);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var item = await ObjectSet.FindAsync(id);
            if(item != null)
            {
                DbContext.Remove(item);
            }
        }

        public virtual async Task<int> SaveChangesAsync() => await DbContext.SaveChangesAsync();

        public async Task<ConveyPaging.PagedResult<T>> BrowseAsync<TQuery>(
            TQuery query,
            ISpecification<T> specification = null,
            CancellationToken ct = default) where TQuery : IPagedQuery
        {
            var results = await Queryable(specification).PaginateAsync(query);
            return results;
        }
    }
}
