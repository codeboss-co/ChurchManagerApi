using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Specifications;
using ChurchManager.Persistence.Models.People;
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

        public async Task<AutocompleteResults> AutocompleteAsync(string searchTerm, CancellationToken ct = default)
        {
            var autocomplete = await Queryable(new PeopleAutocompleteSpecification(searchTerm))
                .AsNoTracking()
                .Select(x => new AutocompleteViewModel(x.Id, x.FullName.ToString()))
                .ToListAsync(ct);

            return new AutocompleteResults(autocomplete);
        }
    }
}
