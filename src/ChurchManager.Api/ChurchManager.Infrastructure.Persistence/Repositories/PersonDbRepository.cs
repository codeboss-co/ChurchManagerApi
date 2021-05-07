using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Domain.Specifications;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
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

        #region Queryable
        /// <summary>
        /// Returns a queryable collection of <see cref="Rock.Model.Person"/> entities (not including Deceased or Nameless records)
        /// </summary>
        /// <returns>A queryable collection of <see cref="Rock.Model.Person"/> entities.</returns>
        public override IQueryable<Person> Queryable()
        {
            return Queryable(new PersonQueryOptions());
        }

        /// <summary>
        /// Returns a queryable collection of <see cref="Person"/> entities 
        /// using the options specified the <see cref="PersonQueryOptions"/> (default is to exclude deceased people and pending person records)
        /// </summary>
        public IQueryable<Person> Queryable(PersonQueryOptions personQueryOptions)
        {
            return Queryable(null, personQueryOptions);
        } 
        #endregion

        public Task<Person> ProfileByUserLoginId(string userLoginId, CancellationToken ct = default)
        {
            return Queryable(new ProfileByUserLoginSpecification(userLoginId))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<PersonDomain> ProfileByPersonId(int personId, bool condensed = false, CancellationToken ct = default)
        {
            var entity = await Queryable(new ProfileByPersonSpecification(personId, condensed))
                .FirstOrDefaultAsync();

            return entity is not null
                ? new PersonDomain(entity)
                : null;
        }

        public Task<UserDetails> UserDetailsByUserLoginId(string userLoginId, CancellationToken ct = default)
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

        public async Task<PagedResult<PersonDomain>> BrowsePeopleAsync(SearchTermQueryParameter query, CancellationToken ct = default)
        {
            // Paging
            var pagedResult = await Queryable()
                .Specify(new BrowsePeopleSpecification(query.SearchTerm))
                /*.Select(x => new PersonBrowseViewModel
                {
                    PersonId = x.Id,
                    FullName = x.FullName,
                    AgeClassification = x.AgeClassification,
                    ConnectionStatus = x.ConnectionStatus,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate,
                    Church = x.Church.Name
                    
                })*/
                .PaginateAsync(query);

            return PagedResult<PersonDomain>.Create(
                pagedResult.Items.Select(entity => new PersonDomain(entity)),
                pagedResult.CurrentPage,
                pagedResult.ResultsPerPage,
                pagedResult.TotalPages,
                pagedResult.TotalResults);
        }

        /// <summary>
        /// Looks for a single exact match based on the critieria provided. If more than one person is found it will return null (consider using FindPersonsAsync).
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="updatePrimaryEmail">if set to <c>true</c> the person's primary email will be updated to the search value if it was found as a person search key (alternate lookup address).</param>
        /// <param name="includeDeceased">if set to <c>true</c> include deceased individuals.</param>
        /// <param name="includeBusinesses">if set to <c>true</c> include businesses records.</param>
        /// <returns></returns>
        public Task<Person> FindPersonAsync(string firstName, string lastName, string email, bool includeDeceased = false, CancellationToken ct = default)
        {
            return FindPersonAsync(new PersonMatchQuery(firstName, lastName, email, string.Empty), includeDeceased, ct);
        }

        /// <summary>
        /// Finds the person.
        /// </summary>
        /// <param name="personMatchQuery">The person match query.</param>

        /// <returns></returns>
        public async Task<Person> FindPersonAsync(PersonMatchQuery personMatchQuery, bool includeDeceased = false, CancellationToken ct = default)
        {
            var matches = await FindPersonsAsync(personMatchQuery, includeDeceased, ct);

            var match = matches.FirstOrDefault();

            return match;
        }

        /// <summary>
        /// Finds people who are considered to be good matches based on the query provided.
        /// </summary>
        /// <returns>A IEnumerable of person, ordered by the likelihood they are a good match for the query.</returns>
        public async Task<IEnumerable<Person>> FindPersonsAsync(PersonMatchQuery query, bool includeDeceased = false, CancellationToken ct = default)
        {
            var queryable = Queryable()
                .AsNoTracking()
                .Where(x =>
                    EF.Functions.ILike(x.FullName.FirstName, $"%{query.FirstName}%") &&
                    EF.Functions.ILike(x.FullName.LastName, $"%{query.LastName}%") &&
                    x.Email != null && x.Email.Address != null &&
                    EF.Functions.ILike(x.Email.Address, $"%{query.LastName}%")
                );

            return await queryable.ToListAsync(ct);
        }


        /// <summary>
        /// Returns a queryable collection of <see cref="Person"/> entities 
        /// using the options specified the <see cref="PersonQueryOptions"/> (default is to exclude deceased people and pending person records)
        /// </summary>
        private IQueryable<Person> Queryable(string includes, PersonQueryOptions personQueryOptions)
        {
            var qry = Queryable(includes);

            personQueryOptions ??= new PersonQueryOptions();

            if(personQueryOptions.IncludeDeceased == false)
            {
                qry = qry.Where(p => p.DeceasedStatus == null || 
                                     (p.DeceasedStatus != null && p.DeceasedStatus.IsDeceased == false));
            }

            if(personQueryOptions.IncludePendingStatus ==  false)
            {
                qry = qry.Where(p => p.RecordStatus == RecordStatus.Active);
            }

            return qry;
        }
    }
}
