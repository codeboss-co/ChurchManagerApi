using Ardalis.GuardClauses;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ConveyPaging = Convey.CQRS.Queries;
using Dynamic = System.Linq.Dynamic.Core;

namespace ChurchManager.Infrastructure.Persistence.Extensions
{
    public static class PagingQueryableExtensions
    {
        /// <summary>
        /// Used for paging with an <see cref="PagedResult{T}"/> object.
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> queryable, IPagedQuery paging)
        {
            Guard.Against.Null(paging, nameof(paging));

            int page = 0, resultsPerPage = 0;

            if(paging.Page <= 0)
            {
                page = 1;
            }

            if(paging.Results <= 0)
            {
                resultsPerPage = 10;
            }

            var skip = (page - 1) * resultsPerPage;

            return queryable.PageBy(skip, paging.Results);
        }

        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            Guard.Against.Null(query, nameof(query));

            return query.Skip(skipCount).Take(maxResultCount);
        }

        public static async Task<ConveyPaging.PagedResult<T>> PaginateAsync<T>(this IQueryable<T> queryable, IPagedQuery paging)
            => await queryable.PaginateAsync(paging.OrderBy, paging.SortOrder, paging.Page, paging.Results);

        public static async Task<ConveyPaging.PagedResult<T>> PaginateAsync<T>(
            this IQueryable<T> queryable,
            string orderBy,
            string sortOrder,
            int page = 1,
            int resultsPerPage = 10,
            CancellationToken ct = default)
        {
            if(page <= 0) { page = 1; }
            if(resultsPerPage <= 0) { resultsPerPage = 10; }

            var isEmpty = await queryable.AnyAsync(ct) == false;
            if(isEmpty)
            {
                return ConveyPaging.PagedResult<T>.Empty;
            }

            // Set Order By
            if(!string.IsNullOrWhiteSpace(orderBy))
            {
                queryable = queryable.OrderBy($"{orderBy} {sortOrder ?? "ascending"}");
            }

            // Calculate totals
            var totalResults = await queryable.CountAsync(ct);
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var skip = (page - 1) * resultsPerPage;

            // Paging
            queryable = queryable.Skip(skip).Take(resultsPerPage);

            var data = await queryable.ToListAsync(ct);

            return ConveyPaging.PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        /// <summary>
        /// Wraps Dynamic code  PagedResults into Convey Paged result
        /// </summary>
        public static async Task<ConveyPaging.PagedResult<T>> Map<T>(this Dynamic.PagedResult<T> pagedQuery, CancellationToken ct = default)
        {
            var convey = ConveyPaging.PagedResult<T>.Create(
                await pagedQuery.Queryable.ToListAsync(ct),
                pagedQuery.CurrentPage,
                pagedQuery.PageSize,
                pagedQuery.PageCount,
                pagedQuery.RowCount);

            return convey;
        }
    }
}
