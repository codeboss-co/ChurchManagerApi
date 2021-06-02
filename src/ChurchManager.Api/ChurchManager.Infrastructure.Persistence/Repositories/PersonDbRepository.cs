using System.Collections.Generic;
using System.Linq;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryBase<Person>, IPersonDbRepository
    {
        public PersonDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Finds people who are considered to be good matches based on the query provided.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <param name="includeDeceased">if set to <c>true</c> [include deceased].</param>
        /// <returns>A IEnumerable of person, ordered by the likelihood they are a good match for the query.</returns>
        public IQueryable<Person> FindPersons(PersonMatchQuery searchParameters, bool includeDeceased = false)
        {
            // Query by last name, suffix, dob, and gender
            var query = Queryable(includeDeceased)
                .AsNoTracking()
                .Where(p => 
                    p.FullName.FirstName == searchParameters.FirstName &&
                    p.FullName.LastName == searchParameters.LastName);

            if (!searchParameters.Email.IsNullOrEmpty())
            {
                query = query.Where(x => x.Email != null && x.Email.Address == searchParameters.Email);
            }

            return query;
        }

        public IQueryable<Person> Queryable(bool includeDeceased)
        {
            return Queryable(new PersonQueryOptions() {IncludeDeceased = includeDeceased});
        }

        public IQueryable<Person> Queryable(PersonQueryOptions personQueryOptions)
        {
            return this.Queryable(null, personQueryOptions);
        }

        private IQueryable<Person> Queryable(string[] includes, PersonQueryOptions personQueryOptions)
        {
            var qry = base.Queryable(includes);

            if (personQueryOptions.IncludePendingStatus == false)
            {
                qry = qry.Where(x => x.RecordStatus != RecordStatus.Pending);
            }

            if (personQueryOptions.IncludeDeceased)
            {
                qry = qry.Where(p => p.DeceasedStatus != null &&
                                     p.DeceasedStatus.IsDeceased.HasValue &&
                                     p.DeceasedStatus.IsDeceased.Value);
            }

            return qry;
        }
    }
}
