using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Shared.Persistence
{
    /// <summary>
    /// Generic CRUD Database Repository
    /// Specific Repositories extend this class
    /// </summary>
    public abstract class CrudDatabaseRepository<T> : ICrudDatabaseRepository<T> where T : class, IAggregateRoot<int>
    {
        #region Fields

        internal DbSet<T> ObjectSet;

        #endregion

        #region Properties

        public DbContext DbContext { get; }

        #endregion

        #region Constructors

        public CrudDatabaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            ObjectSet = DbContext.Set<T>();
        }

        #endregion

        public IQueryable<T> Queryable() => ObjectSet;

        public virtual IQueryable<T> Queryable(ISpecification<T> specification)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = specification.Includes
                .Aggregate(ObjectSet.AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = specification.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            if (specification.Criteria != null)
            {
                return secondaryResult.Where(specification.Criteria);
            }

            return secondaryResult;
        }

        #region Repository Methods

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await Queryable().AsNoTracking().ToListAsync();

        public virtual async Task<T> GetByIdAsync(int id) => await ObjectSet.FindAsync(id);

        public virtual async Task<int> AddAsync(T item)
        {
            await ObjectSet.AddAsync(item);

            return item.Id;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> items) => await ObjectSet.AddRangeAsync(items);

        public virtual async Task UpdateAsync(int id, T sourceItem)
        {
            var targetItem = await ObjectSet.FindAsync(id);
            if(targetItem != null)
            {
                DbContext.Entry(targetItem).CurrentValues.SetValues(sourceItem);
            }
        }
        
        public virtual async Task Delete(int id)
        {
            var item = await ObjectSet.FindAsync(id);
            if(item != null)
            {
                DbContext.Remove(item);
            }
        }

        public virtual async Task<int> SaveChangesAsync() => await DbContext.SaveChangesAsync();

        #endregion
    }
}
