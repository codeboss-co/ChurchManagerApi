using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Codeboss.Types;

namespace ChurchManager.Shared.Persistence
{
    public interface ISpecification<T> where T : class, IAggregateRoot<int>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }

    public abstract class Specification<T> : ISpecification<T> where T : class, IAggregateRoot<int>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
