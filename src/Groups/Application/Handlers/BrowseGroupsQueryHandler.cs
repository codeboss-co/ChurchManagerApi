using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core;
using Contracts;
using Convey.CQRS.Queries;
using Domain.Repositories;
using CBQuery = CodeBoss.CQRS.Queries;

namespace Application.Handlers
{
    public record BrowseGroupsQuery(
        string SearchTerm,
        int? PersonId,
        int Page,
        int Results,
        string OrderBy,
        string SortOrder) : CBQuery.IQuery<GroupViewModel>, IPagedQuery {}

    public class BrowseGroupsQueryHandler : CBQuery.IQueryHandler<BrowseGroupsQuery, GroupViewModel>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IGroupDbRepository _groupDbRepository;

        public BrowseGroupsQueryHandler(ICognitoCurrentUser currentUser, IGroupDbRepository groupDbRepository)
        {
            _currentUser = currentUser;
            _groupDbRepository = groupDbRepository;
        }

        public async Task<GroupViewModel> HandleAsync(BrowseGroupsQuery query, CancellationToken ct = default)
        {
            // Allows searching for current Person or specific persons groups
            var personId = query.PersonId ??_currentUser.CurrentPerson.Value.Result.PersonId;
            var pagedResult = await _groupDbRepository.BrowsePersonsGroups(personId, query.SearchTerm, query, ct);

            return new GroupViewModel(pagedResult);
        }
    }
}
