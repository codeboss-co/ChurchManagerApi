using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using Domain.Model;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using People.Domain.Model;
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

            var groupsDomain = groups.Select(x => new GroupDomain(x));
            var membersPersonId = groups.SelectMany(x => x.Members.Select(x => x.PersonId)).ToList();

            var personDetails = await _personDbRepository.Queryable().Where(x => membersPersonId.Contains(x.Id)).ToListAsync(cancellationToken);
            var personDomainList = personDetails.Select(x => new PersonDomain(x));

            var studentList = groupsDomain.SelectMany(x => x.Members).ToList();

            var membersJoined = studentList.Join(
                personDomainList,
                groupMember => groupMember.PersonId,
                person => Int32.Parse(person["PersonId"]),
                (g, p) => new { g.GroupId, Person = p, Member = g})
                .GroupBy(x => x.GroupId);

            var results = groupsDomain.Join(
                membersJoined,
                group => group.GroupId,
                member => member.Key,
                (g, grouping) => new { g.Name, g.GroupType, g.Description, Members = grouping }
                );

            return new GroupsForPerson(results);
        }
    }
}
