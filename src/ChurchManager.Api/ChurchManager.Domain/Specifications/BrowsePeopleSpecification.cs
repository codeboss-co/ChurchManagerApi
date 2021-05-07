using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Specifications
{
    public class BrowsePeopleSpecification : Specification<Person>
    {
        private const int MinSearchTermLength = 3;

        public BrowsePeopleSpecification(string searchTerm)
        {
            Criteria = person =>
                // Name Search
                EF.Functions.ILike(person.FullName.FirstName, $"%{searchTerm}%") ||
                EF.Functions.ILike(person.FullName.LastName, $"%{searchTerm}%");

            Includes.Add(person => person.Church);
            Includes.Add(person => person.PhoneNumbers);
        }
    }
}
