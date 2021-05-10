﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Specifications;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using ConveyPaging = Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryAsync<Person>, IPersonDbRepository
    {

        public PersonDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
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

        public Task<Person> ProfileByPersonId(int personId, bool condensed = false, CancellationToken ct = default)
        {
            return Queryable(new ProfileByPersonSpecification(personId, condensed))
                .FirstOrDefaultAsync(ct);
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
                .FirstOrDefaultAsync(ct);
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

        public async Task<ConveyPaging.PagedResult<Person>> BrowsePeopleAsync(SearchTermQueryParameter query, CancellationToken ct = default)
        {
            var queryable = Queryable()
                .AsNoTracking()
                .Specify(new BrowsePeopleSpecification(query.SearchTerm));

            if (!query.OrderBy.IsNullOrEmpty())
            {
                queryable = queryable.OrderBy($"{query.OrderBy} {query.SortOrder ?? "ascending"}");
            }

            // Paging
            var pagedQuery = queryable
                .Page(query.Page, query.Results)
                .PageResult(query.Page, query.Results);

            return await pagedQuery.Map(ct);
        }

        /// <summary>
        /// Looks for a single exact match based on the critieria provided. If more than one person is found it will return null (consider using FindPersonsAsync).
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="includeDeceased">if set to <c>true</c> include deceased individuals.</param>
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
