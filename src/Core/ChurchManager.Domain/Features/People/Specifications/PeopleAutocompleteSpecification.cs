using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class PeopleAutocompleteSpecification : Specification<Person, PeopleAutocompleteViewModel>
    {
        public PeopleAutocompleteSpecification(string searchTerm)
        {
            Query.AsNoTracking();

            // If the autocomplete has prepopulated name and surname
            // We want to match against that
            // It will be in the format "FirstName LastName"
            if (searchTerm.Contains(" "))
            {
                var splitSearch = searchTerm.Split(' ');
                // We look for where the FirstName and LastName both match
                Query.Where(x =>
                    EF.Functions.ILike(x.FullName.FirstName, $"%{splitSearch[0]}%") &&
                    EF.Functions.ILike(x.FullName.LastName, $"%{splitSearch[1]}%")
                );
            } 
            else
            {
                // Any match will do here
                Query.Where(x =>
                    EF.Functions.ILike(x.FullName.FirstName, $"%{searchTerm}%") ||
                    EF.Functions.ILike(x.FullName.MiddleName, $"%{searchTerm}%") ||
                    EF.Functions.ILike(x.FullName.LastName, $"%{searchTerm}%")
                );
            }

            Query.Select(x => new PeopleAutocompleteViewModel(x.Id, x.FullName.ToString(), x.PhotoUrl, x.ConnectionStatus));
        }
    }
}
