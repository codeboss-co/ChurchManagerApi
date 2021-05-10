using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class PeopleAutocompleteSpecification : Specification<Person, PeopleAutocompleteViewModel>
    {
        public PeopleAutocompleteSpecification(string searchTerm)
        {
            Query.AsNoTracking();

            Query.Where(x =>
                EF.Functions.ILike(x.FullName.FirstName, $"%{searchTerm}%") ||
                EF.Functions.ILike(x.FullName.MiddleName, $"%{searchTerm}%") ||
                EF.Functions.ILike(x.FullName.LastName, $"%{searchTerm}%")
            );

            Query.Select(x => new PeopleAutocompleteViewModel(x.Id, x.FullName.ToString(), x.PhotoUrl, x.ConnectionStatus));
        }
    }
}
