using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Model;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository : IGenericRepositoryAsync<Person>
    {
        Task<Person> ProfileByUserLoginId(string userLoginId, CancellationToken ct = default);
        Task<PersonDomain> ProfileByPersonId(int personId, bool condensed = false, CancellationToken ct = default);
        Task<UserDetails> UserDetailsByUserLoginId(string queryUserLoginId, CancellationToken ct = default);
        Task<PeopleAutocompleteResults> AutocompleteAsync(string searchTerm, CancellationToken ct = default);
        Task<PagedResult<PersonDomain>> BrowsePeopleAsync(SearchTermQueryParameter query, CancellationToken ct = default);

        // Find People
        Task<Person> FindPersonAsync(string firstName, string lastName, string email, bool includeDeceased = false, CancellationToken ct = default);
        Task<Person> FindPersonAsync(PersonMatchQuery personMatchQuery, bool includeDeceased = false, CancellationToken ct = default);
        Task<IEnumerable<Person>> FindPersonsAsync(PersonMatchQuery query, bool includeDeceased = false, CancellationToken ct = default);
    }
}
