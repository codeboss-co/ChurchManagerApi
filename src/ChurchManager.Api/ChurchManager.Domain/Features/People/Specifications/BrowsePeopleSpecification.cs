using System;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.Specification;
using ChurchManager.Domain.Parameters;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class BrowsePeopleSpecification : Specification<Person>
    {
        public BrowsePeopleSpecification(SearchTermQueryParameter query)
        {
            Query
                .Where(person =>
                    // Name Search
                    EF.Functions.ILike(person.FullName.FirstName, $"%{query.SearchTerm}%") ||
                    EF.Functions.ILike(person.FullName.LastName, $"%{query.SearchTerm}%"))

                .Include(person => person.Church)
                .Include(person => person.PhoneNumbers);

            if (!query.OrderBy.IsNullOrEmpty())
            {
                if(query.SortOrder == "ascending" || query.SortOrder == "ASC")
                {
                    Query.OrderBy(ToLambda<Person, Object>(query.OrderBy));
                }
                else
                {
                    Query.OrderByDescending(ToLambda<Person, Object>(query.OrderBy));
                }
            }

            var skip = (query.Page - 1) * query.Results;
            // Paging
            Query
                .Skip(skip)
                .Take(query.Results);
        }

        /// <summary>
        /// Converts to lambda.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public Expression<Func<TEntity, TResult>> ToLambda<TEntity, TResult>(string prop)
        {
            var param = Expression.Parameter(typeof(TEntity), "p");
            var parts = prop.Split('.');

            Expression parent = parts.Aggregate<string, Expression>(param, Expression.Property);
            Expression conversion = Expression.Convert(parent, typeof(object));

            return Expression.Lambda<Func<TEntity, TResult>>(conversion, param);
        }
    }
}
