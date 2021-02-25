using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using Domain.Model;
using Domain.Repositories;
using People.Domain.Repositories;

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
        private readonly IPersonDbRepository _personDbRepository;

        public GroupsForPersonQueryHandler(IGroupDbRepository groupDbRepository, IPersonDbRepository personDbRepository)
        {
            _groupDbRepository = groupDbRepository;
            _personDbRepository = personDbRepository;
        }

        public async Task<GroupsForPerson> HandleAsync(GroupsForPersonQuery query, CancellationToken cancellationToken = default)
        {
            var groups = await _groupDbRepository.AllPersonsGroups(query.PersonId);

            var groupsDomain = groups.ToList();
           
            return new GroupsForPerson(groupsDomain.Select(x => new GroupDomain(x)));
        }
    }
}
