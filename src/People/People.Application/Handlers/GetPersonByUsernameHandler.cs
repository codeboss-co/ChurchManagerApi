using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using People.Contracts;
using People.Domain.Repositories;

namespace People.Application.Handlers
{
    public record UserPerson(dynamic Result)
    {
    }

    public record PersonByUsernameQuery(string UserLoginId) : IQuery<UserPerson>
    {
    }

    public class GetPersonByUsernameHandler : IQueryHandler<PersonByUsernameQuery, UserPerson>
    {
        private readonly IPersonDbRepository _personDbRepository;

        public GetPersonByUsernameHandler(IPersonDbRepository personDbRepository)
        {
            _personDbRepository = personDbRepository;
        }

        public async Task<UserPerson> HandleAsync(PersonByUsernameQuery query, CancellationToken ct = default)
        {
            var person = new PersonViewModel(await _personDbRepository.PersonByUserLoginId(query.UserLoginId));

            return new UserPerson(person);
        }
    }
}
