using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

// https://www.npgsql.org/efcore/misc/collations-and-case-sensitivity.html?tabs=data-annotations#ilike
namespace ChurchManager.Domain.Specifications
{
    public class PeopleAutocompleteSpecification : Specification<Person>
    {
        private const int MinSearchTermLength = 3;

        public PeopleAutocompleteSpecification(string searchTerm)
        {
            if (!searchTerm.IsNullOrEmpty() && searchTerm.Length > MinSearchTermLength)
            {
                Criteria = person =>

                    // Name Search
                    EF.Functions.ILike(person.FullName.FirstName, $"%{searchTerm}%");
                /*person.FullName.FirstName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                person.FullName.MiddleName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                person.FullName.LastName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||*/

                // Email Search
                /*(
                    person.Email != null &&
                    person.Email.Address != null &&
                    person.Email.Address.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)
                );*/
            }
        }
    }
}
