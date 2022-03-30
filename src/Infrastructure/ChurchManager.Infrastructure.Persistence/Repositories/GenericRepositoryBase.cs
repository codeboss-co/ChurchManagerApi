using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryBase<T> : RepositoryBase<T>, IGenericDbRepository<T> where T : class, IAggregateRoot<int>
    {
        #region Fields

        internal DbSet<T> ObjectSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        #endregion

        #region Properties

        public DbContext DbContext { get; }

        #endregion

        public GenericRepositoryBase(DbContext dbContext)
            : this(dbContext, SpecificationEvaluator.Default)
        {
        }

        /// <inheritdoc/>
        public GenericRepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
            : base(dbContext, specificationEvaluator)
        {
            DbContext = dbContext;
            ObjectSet = DbContext.Set<T>();
            _specificationEvaluator = specificationEvaluator;
        }

        public virtual IQueryable<T> Queryable(params string[] includes) => QueryableIncludes(ObjectSet, includes);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await ObjectSet.AddRangeAsync(entities, cancellationToken);

            await SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<PagedResult<T>> BrowseAsync(IPagedQuery query, ISpecification<T> specification, CancellationToken ct = default)
        {
            var list = await ListAsync(specification, ct);
            var totalResults = await CountAsync(specification, ct);
            var totalPages = (int)Math.Ceiling((decimal)totalResults / query.Results);

            var pagedResult = PagedResult<T>.Create(list, query.Page, query.Results, totalPages, totalResults);

            return pagedResult;
        }

        /// <inheritdoc/>
        public virtual async Task<PagedResult<TResult>> BrowseAsync<TResult>(IPagedQuery query, ISpecification<T, TResult> specification, CancellationToken ct = default)
        {
            var list = await ListAsync<TResult>(specification, ct);
            var totalResults = await CountAsync(specification, ct);
            var totalPages = (int)Math.Ceiling((decimal)totalResults / query.Results);

            var pagedResult = PagedResult<TResult>.Create(list, query.Page, query.Results, totalPages, totalResults);

            return pagedResult;
        }

        /// <summary>
        /// Applies a comma-delimited list of includes to the Queryable
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        private IQueryable<T> QueryableIncludes(IQueryable<T> query, params string[] includes)
        {
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
