using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
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
