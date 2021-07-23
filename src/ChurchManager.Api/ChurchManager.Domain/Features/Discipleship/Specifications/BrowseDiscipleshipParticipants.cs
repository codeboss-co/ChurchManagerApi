using System;
using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Discipleship.Specifications
{
    public class BrowseDiscipleshipParticipants : Specification<DiscipleshipStep>, ISingleResultSpecification
    {
        public BrowseDiscipleshipParticipants(
            IPagedQuery paging,
            int stepDefinitionId, string status, DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include(x => x.Person);

            Query.Where(g => g.Id == stepDefinitionId);

            // Status Filder
            if (!status.IsNullOrEmpty())
            {
                Query.Where(g => g.Status == status);
            }

            // Date Filters
            if(from.HasValue)
            {
                Query.Where(g => g.CreatedDate >= from.Value);
            }
            if(to.HasValue)
            {
                Query.Where(g => g.CreatedDate <= to.Value);
            }

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());
        }
    }
}
