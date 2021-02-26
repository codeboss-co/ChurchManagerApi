using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Shared.Persistence
{
    public static class PagingQueryableExtensions
    {
        /// <summary>
        /// Used for paging with an <see cref="PagedResult{T}"/> object.
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> queryable, IPagedQuery paging)
        {
            Guard.Against.Null(paging, nameof(paging));

            return queryable.PageBy(paging.Page, paging.Results);
        }

        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            Guard.Against.Null(query, nameof(query));

            return query.Skip(skipCount).Take(maxResultCount);
        }

        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> queryable, IPagedQuery paging)
            => await queryable.PaginateAsync(paging.OrderBy, paging.SortOrder, paging.Page, paging.Results);

        public static async Task<PagedResult<T>> PaginateAsync<T>(
            this IQueryable<T> queryable,
            string orderBy,
            string sortOrder,
            int page = 1,
            int resultsPerPage = 10)
        {
            if(page <= 0)
            {
                page = 1;
            }

            if(resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }

            var isEmpty = await queryable.AnyAsync() == false;
            if(isEmpty)
            {
                return PagedResult<T>.Empty;
            }

            var totalResults = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var skip = (page - 1) * resultsPerPage;

            List<T> data;
            if(string.IsNullOrWhiteSpace(orderBy))
            {
                
                data = await queryable.PageBy(skip, resultsPerPage).ToListAsync();
                return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
            }

            if(sortOrder?.ToLowerInvariant() == "asc")
            {
                data = await queryable.OrderBy(ToLambda<T>(orderBy)).PageBy(skip, resultsPerPage).ToListAsync();
            }
            else
            {
                data = await queryable.OrderByDescending(ToLambda<T>(orderBy)).PageBy(skip, resultsPerPage).ToListAsync();
            }

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
