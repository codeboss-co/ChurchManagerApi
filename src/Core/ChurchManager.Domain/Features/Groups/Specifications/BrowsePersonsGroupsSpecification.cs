using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Parameters;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class BrowsePersonsGroupsSpecification : Specification<Group>
    {
        public BrowsePersonsGroupsSpecification(int personId, SearchTermQueryParameter query)
        {
            Query.AsNoTracking();

            // The groups this person is the leader of
            Query.Where(g =>
                g.Members
                    .Any(m => m.PersonId == personId && m.GroupRole.IsLeader));

            // Filter the groups
            if(!string.IsNullOrEmpty(query.SearchTerm))
            {
                // Search By Name Or Description
                Query
                    .Search(x => x.Name, "%" + query.SearchTerm + "%")
                    .Search(x => x.Description, "%" + query.SearchTerm + "%")
                    ;
            }

            if(!query.OrderBy.IsNullOrEmpty())
            {
                if(query.SortOrder == "ascending" || query.SortOrder == "ASC")
                {
                    Query.OrderBy(query.OrderBy);
                }
                else
                {
                    Query.OrderByDescending(query.OrderBy);
                }
            }

            Query.Include(x => x.GroupType);
            Query.Include(x => x.Members)
                .ThenInclude(x => x.GroupRole);
        }
    }
}
