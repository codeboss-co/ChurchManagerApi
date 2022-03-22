using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class BrowseFamiliesSpecification : Specification<Family, FamilyViewModel>
    {
        public BrowseFamiliesSpecification(
            IPagedQuery paging,
            string name,
            string address)
        {
            Query.AsNoTracking();


            // Name Filter
            if(!name.IsNullOrEmpty())
            {
                Query.Where(x => EF.Functions.ILike(x.Name, $"%{name}%"));
            }

            // Address Filter
            if (!address.IsNullOrEmpty())
            {
                // Any address match
                Query.Where(x =>
                    EF.Functions.ILike(x.Address.Street, $"%{address}%") ||
                    EF.Functions.ILike(x.Address.City, $"%{address}%") ||
                    EF.Functions.ILike(x.Address.Province, $"%{address}%") ||
                    EF.Functions.ILike(x.Address.Country, $"%{address}%")
                );
            }

            Query.OrderBy(x => x.Name);

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());

            Query.Select(x => new FamilyViewModel
            {
                Id = x.Id,
                Name = x.Name,
                City = x.Address.City,
                Country = x.Address.Country,
                PostalCode = x.Address.PostalCode,
                Street = x.Address.Street,
                Province = x.Address.Province
            });
        }
    }
}
