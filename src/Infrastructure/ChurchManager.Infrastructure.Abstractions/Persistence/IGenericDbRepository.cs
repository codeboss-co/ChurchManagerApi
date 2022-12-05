using Ardalis.Specification;
using Codeboss.Types;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Abstractions.Persistence
{
    public interface IGenericDbRepository<T> : IRepositoryBase<T>, IPaginatedDbRepository<T>
        where T : class, IAggregateRoot<int>
    {
        DbContext DbContext { get; }
        IQueryable<T> Queryable(params string[] includes);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }

    public interface IPaginatedDbRepository<T> where T : class, Codeboss.Types.IEntity<int>
    {
        /// <summary>
        /// Paging all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<PagedResult<T>> BrowseAsync(IPagedQuery query, ISpecification<T> specification, CancellationToken ct = default);

        /// <summary>
        /// Paging of all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// <para>
        /// Projects each entity into a new form, being <typeparamref name="TResult" />.
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
        /// <param name="query">Paging query</param>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
        /// </returns>
        Task<PagedResult<TResult>> BrowseAsync<TResult>(IPagedQuery query, ISpecification<T, TResult> specification, CancellationToken ct = default);
    }
}
