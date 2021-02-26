using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;

namespace Application.Handlers
{
    public record BrowseGroups(dynamic Result)
    {
    }

    public record BrowseGroupsQuery(int PersonId) : IQuery<BrowseGroups>
    {
        // Filtering
        public string SearchTerm { get; set; }

        // Paging
        public int Page { get; set; }
        public int Results { get; set; }
        public string OrderBy { get; set; }
        public string SortOrder { get; set; }
    }

    public class BrowseGroupsQueryHandler : IQueryHandler<BrowseGroupsQuery, BrowseGroups>
    {
        public BrowseGroupsQueryHandler()
        {
            
        }

        public Task<BrowseGroups> HandleAsync(BrowseGroupsQuery query, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
