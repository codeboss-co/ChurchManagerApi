using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using Contracts;
using Domain.Repositories;

namespace Application.Handlers
{
    public record GroupsForPerson(dynamic Result)
    {
    }

    public record GroupsForPersonQuery(int PersonId) : IQuery<GroupsForPerson>
    {
    }

    public class GroupsForPersonQueryHandler : IQueryHandler<GroupsForPersonQuery, GroupsForPerson>
    {
        private readonly IGroupDbRepository _groupDbRepository;

        public GroupsForPersonQueryHandler(IGroupDbRepository groupDbRepository)
        {
            _groupDbRepository = groupDbRepository;
        }

        public async Task<GroupsForPerson> HandleAsync(GroupsForPersonQuery query, CancellationToken ct = default)
        {
            var groups = await _groupDbRepository.AllPersonsGroups(query.PersonId, ct);

            return new GroupsForPerson(groups.Select(x => new GroupViewModel(x)));
        }
    }
}
