using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using Domain.Repositories;

namespace Application.Handlers
{
    public class GroupsForPerson
    {
        public int Groups { get; set; }
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

        public async Task<GroupsForPerson> HandleAsync(GroupsForPersonQuery query, CancellationToken cancellationToken = default)
        {
            var groups = await _groupDbRepository.AllPersonsGroups(query.PersonId);

            return new GroupsForPerson();
        }
    }
}
