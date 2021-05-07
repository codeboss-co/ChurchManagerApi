﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Model;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository : IGenericRepositoryAsync<Person>
    {
        IQueryable<Person> ProfileByUserLoginId(string userLoginId);
        Task<PersonDomain> ProfileByPersonId(int personId, bool condensed = false);
        Task<UserDetails> UserDetailsByUserLoginId(string queryUserLoginId);
        Task<PeopleAutocompleteResults> AutocompleteAsync(string searchTerm, CancellationToken ct = default);
        Task<PagedResult<PersonDomain>> BrowsePeopleAsync(SearchTermQueryParameter query, CancellationToken ct = default);

        // Find People
        Task<Person> FindPersonAsync(string firstName, string lastName, string email, bool includeDeceased = false, CancellationToken ct = default);
        Task<Person> FindPersonAsync(PersonMatchQuery personMatchQuery, bool includeDeceased = false, CancellationToken ct = default);
        Task<IEnumerable<Person>> FindPersonsAsync(PersonMatchQuery query, bool includeDeceased = false, CancellationToken ct = default);
    }
}
