using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class BrowsePeopleSpecification : Specification<Person>
    {
        public BrowsePeopleSpecification(string searchTerm)
        {
            Query
                .Where(person =>
                    // Name Search
                    EF.Functions.ILike(person.FullName.FirstName, $"%{searchTerm}%") ||
                    EF.Functions.ILike(person.FullName.LastName, $"%{searchTerm}%"))

                .Include(person => person.Church)
                .Include(person => person.PhoneNumbers);

            Query.Ord
        }
    }
}
