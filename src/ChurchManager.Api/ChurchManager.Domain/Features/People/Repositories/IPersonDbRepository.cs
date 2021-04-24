using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.People;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository : IGenericRepositoryAsync<Person>
    {
        Task<PersonDomain> ProfileByUserLoginId(string userLoginId);
        Task<PersonDomain> ProfileByPersonId(int personId);
        Task<UserDetails> UserDetailsByUserLoginId(string queryUserLoginId);
        Task<PeopleAutocompleteResults> AutocompleteAsync(string searchTerm, CancellationToken ct = default);
        Task<PagedResult<object>> BrowsePeopleAsync(SearchTermQueryParameter query, CancellationToken ct = default);
    }
}
