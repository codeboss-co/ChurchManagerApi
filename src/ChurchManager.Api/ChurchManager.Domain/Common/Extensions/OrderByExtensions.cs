using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.Specification;

namespace ChurchManager.Domain.Common.Extensions
{
    public static class OrderByExtensions
    {
        /// <summary>
        /// Converts to lambda.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static Expression<Func<T, object>> ToLambda<T>(this string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static Expression<Func<TEntity, TResult>> ToLambda<TEntity, TResult>(this string prop)
        {
            var param = Expression.Parameter(typeof(TEntity), "p");
            var parts = prop.Split('.');

            Expression parent = parts.Aggregate<string, Expression>(param, Expression.Property);
            Expression conversion = Expression.Convert(parent, typeof(object));

            return Expression.Lambda<Func<TEntity, TResult>>(conversion, param);
        }


        public static IOrderedSpecificationBuilder<T> OrderBy<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string propertyName)
        {
            Expression<Func<T, object>> orderExpression = propertyName.ToLambda<T, object>();

            return specificationBuilder.OrderBy(orderExpression);
        }

        public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string propertyName)
        {
            Expression<Func<T, object>> orderExpression = propertyName.ToLambda<T, object>();

            return specificationBuilder.OrderByDescending(orderExpression);
        }
    }
}
