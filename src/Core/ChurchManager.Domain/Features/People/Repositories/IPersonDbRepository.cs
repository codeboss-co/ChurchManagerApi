using System.Linq;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository : IGenericDbRepository<Person>
    {
        IQueryable<Person> FindPersons(PersonMatchQuery searchParameters, bool includeDeceased = false);
        IQueryable<Person> Queryable(bool includeDeceased);
        IQueryable<Person> Queryable(PersonQueryOptions personQueryOptions);
    }
}
