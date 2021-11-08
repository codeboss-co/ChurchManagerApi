using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using Convey.CQRS.Queries;
using System;
using System.Linq;

namespace ChurchManager.Domain.Features.Missions.Specifications
{
    public class BrowseMissionsSpecification : Specification<Mission, MissionViewModel>
    {
        public BrowseMissionsSpecification(
            IPagedQuery paging,
            int? personId,
            int? groupId,
            int? churchId,
            string[] types,
            string[] categories,
            DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include(x => x.Person);
            Query.Include(x => x.Group);
            Query.Include(x => x.Church);

            // Person Filter
            if(personId.HasValue)
            {
                Query.Where(g => g.PersonId == personId);
            }

            // Group Filter
            if(groupId.HasValue)
            {
                Query.Where(g => g.GroupId == groupId);
            }

            // Church Filter
            if (churchId.HasValue)
            {
                Query.Where(g => g.ChurchId == churchId);
            }

            // Type Filter
            if (types is not null && types.Any())
            {
                Query.Where(g => types.Contains(g.Type));
            }

            // Category Filter
            if (categories is not null && categories.Any())
            {
                Query.Where(g => categories.Contains(g.Category));
            }


            // Date Filters
            if(from.HasValue)
            {
                Query.Where(g => g.StartDateTime >= from.Value);
            }
            if(to.HasValue)
            {
                Query.Where(g => g.EndDateTime <= to.Value);
            }

            Query.OrderBy(x => x.StartDateTime);

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());

            Query.Select(x => new MissionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Type = x.Type,
                Category = x.Category,
                IconCssClass = x.IconCssClass,
                StartDateTime = x.StartDateTime,
                EndDateTime = x.EndDateTime,
                Person = x.PersonId != null ? new PeopleAutocompleteViewModel(x.PersonId, x.Person.FullName.ToString(), x.Person.PhotoUrl, x.Person.ConnectionStatus) : null,
                Church = x.ChurchId != null ? new AutocompleteResult(x.Church.Id, x.Church.Name) : null,
                Group = x.GroupId != null ? new AutocompleteResult(x.Group.Id, x.Group.Name) : null
            });
        }
    }
}
