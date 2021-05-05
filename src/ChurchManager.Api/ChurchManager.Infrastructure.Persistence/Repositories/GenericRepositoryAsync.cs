using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
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

        public IChurchManagerDbContext DbContext { get; }

        #endregion

        #region Constructors

        public GenericRepositoryAsync(IChurchManagerDbContext dbContext)
        {
            DbContext = dbContext;
            ObjectSet = DbContext.Set<T>();
        }
        #endregion

        #region Queryable

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

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> list of all models
        /// with eager loading of the comma-delimited properties specified in includes
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> Queryable(string includes)
        {
            return QueryableIncludes(ObjectSet, includes);
        }

        /// <summary>
        /// Applies a comma-delimited list of includes to the Queryable
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        private IQueryable<T> QueryableIncludes(IQueryable<T> query, string includes)
        {
            if(!string.IsNullOrEmpty(includes))
            {
                foreach(var include in SplitDelimitedValues(includes))
                {
                    query = query.Include(include);
                }
            }
            return query;
        } 

        #endregion

        public virtual async Task<T> GetByIdAsync(int id) => await ObjectSet.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await ObjectSet.AsNoTracking().ToListAsync();

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

        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            return ObjectSet.AddRangeAsync(entities);
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



        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited by any combination of whitespace, comma, semi-colon, or pipe characters.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="whitespace">if set to <c>true</c> whitespace will be treated as a delimiter</param>
        /// <returns></returns>
        private static string[] SplitDelimitedValues(string str, bool whitespace = true)
        {
            if(str == null)
            {
                return new string[0];
            }

            string regex = whitespace ? @"[\s\|,;]+" : @"[\|,;]+";

            char[] delimiter = new char[] { ',' };
            return Regex.Replace(str, regex, ",").Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
