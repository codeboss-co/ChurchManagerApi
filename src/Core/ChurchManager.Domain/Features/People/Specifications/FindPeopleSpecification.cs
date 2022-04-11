using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class FindPeopleSpecification : Specification<Person>
    {
        public FindPeopleSpecification(PersonMatchQuery query, PersonQueryOptions options = null, params string[] includes)
        {
            options ??= new PersonQueryOptions();

            Query.AsNoTracking();

            foreach(var include in includes)
            {
                Query.Include(include);
            }

            Query.Where(x =>
                EF.Functions.ILike(x.FullName.FirstName, $"%{query.FirstName}%") &&
                EF.Functions.ILike(x.FullName.LastName, $"%{query.LastName}%") &&
                x.Email != null && x.Email.Address != null &&
                EF.Functions.ILike(x.Email.Address, $"%{query.Email}%")
            );

            Query.OrderBy(x => x.FullName.LastName);

            if(options.IncludeDeceased == false)
            {
                Query.Where(p => p.DeceasedStatus == null ||
                                     (p.DeceasedStatus != null && p.DeceasedStatus.IsDeceased == false));
            }

            if(options.IncludePendingStatus == false)
            {
                Query.Where(p => p.RecordStatus != RecordStatus.Pending);
            }
        }
    }
}
