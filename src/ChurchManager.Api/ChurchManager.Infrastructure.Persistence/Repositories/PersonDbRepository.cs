using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using ChurchManager.Infrastructure.Persistence.Specifications;
using ChurchManager.Persistence.Models.People;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryAsync<Person>, IPersonDbRepository
    {
        private readonly IDataShapeHelper<Person> _dataShaper;

        public PersonDbRepository(
            ChurchManagerDbContext dbContext,
            IDataShapeHelper<Person> dataShaper) : base(dbContext)
        {
            _dataShaper = dataShaper;
        }

        public async Task<PersonDomain> ProfileByUserLoginId(string userLoginId)
        {
            var entity = await Queryable(new ProfileByUserLoginSpecification(userLoginId))
                .FirstOrDefaultAsync();

            return entity is not null
                ? new PersonDomain(entity)
                : null;
        }

        public async Task<PersonDomain> ProfileByPersonId(int personId)
        {
            var entity = await Queryable(new ProfileByPersonSpecification(personId))
                .FirstOrDefaultAsync();

            return entity is not null
                ? new PersonDomain(entity)
                : null;
        }

        public Task<UserDetails> UserDetailsByUserLoginId(string userLoginId)
        {
            return Queryable()
                .AsNoTracking()
                .Where(x => x.UserLoginId == userLoginId)
                .Select(x => new UserDetails
                {
                    UserLoginId = x.UserLoginId,
                    FirstName = x.FullName.FirstName,
                    LastName = x.FullName.LastName,
                    Email = x.Email.Address,
                    PhotoUrl = x.PhotoUrl
                })
                .FirstOrDefaultAsync();
        }

        // https://www.npgsql.org/efcore/misc/collations-and-case-sensitivity.html?tabs=data-annotations#ilike
        // https://stackoverflow.com/questions/45708715/entity-framework-ef-functions-like-vs-string-contains
        public async Task<PeopleAutocompleteResults> AutocompleteAsync(string searchTerm, CancellationToken ct = default)
        {
            var autocomplete = await Queryable()
                .AsNoTracking()
                .Where(x => 
                    EF.Functions.ILike(x.FullName.FirstName, $"%{searchTerm}%") ||
                    EF.Functions.ILike(x.FullName.MiddleName, $"%{searchTerm}%") ||
                    EF.Functions.ILike(x.FullName.LastName, $"%{searchTerm}%")
                )
                .Select(x => new PeopleAutocompleteViewModel(x.Id, x.FullName.ToString(), x.PhotoUrl, x.ConnectionStatus))
                .ToListAsync(ct);

            return new PeopleAutocompleteResults(autocomplete);
        }

        public async Task<PagedResult<object>> BrowsePeopleAsync(SearchTermQueryParameter query, CancellationToken ct = default)
        {
            // Paging
            var pagedResult = await Queryable()
                .Specify(new BrowsePeopleSpecification(query.SearchTerm))
                .PaginateAsync(query);

            return PagedResult<object>.Create(
                pagedResult.Items.Select(x => new 
                {
                    x.Id,
                    Name = x.FullName.FirstName + " " + x.FullName.LastName,
                    Email = x.Email?.Address,
                    PhoneNumber = x.PhoneNumbers.FirstOrDefault()?.Number,
                    x.ConnectionStatus,
                    x.PhotoUrl
                }),
                pagedResult.CurrentPage,
                pagedResult.ResultsPerPage,
                pagedResult.TotalPages,
                pagedResult.TotalResults);
        }
    }
}
