using System.Linq;
using System.Linq.Dynamic.Core;
using ChurchManager.Domain.Parameters;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Extensions
{
    // https://gunnarpeipman.com/ef-core-query-specification/
    public static class QuerySpecificationExtensions
    {
        public static IQueryable<T> Specify<T>(
            this IQueryable<T> queryable,
            ISpecification<T> specification) where T : class, IAggregateRoot<int>
        {
            if(specification is null)
            {
                return queryable;
            }

            // Fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = specification.Includes
                .Aggregate(queryable,
                    (current, include) => current.Include(include));

            // Modify the IQueryable to include any string-based include statements
            var secondaryResult = specification.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            
            if(specification.Criteria != null)
            {
                return secondaryResult.Where(specification.Criteria);
            }

            return secondaryResult;
        }
        
        public static IQueryable<T> FieldLimit<T>(this IQueryable<T> queryable, QueryParameter query = null)
        {
            // Limit Query Fields
            if(query is not null && !string.IsNullOrWhiteSpace(query.Fields))
            {
                queryable = queryable.Select<T>("new(" + query.Fields + ")");
            }

            return queryable;
        }
    }
}
