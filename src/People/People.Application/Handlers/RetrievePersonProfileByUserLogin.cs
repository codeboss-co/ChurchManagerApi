using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using People.Contracts;
using People.Domain.Repositories;

namespace People.Application.Handlers
{
    public record PersonProfile(dynamic Result)
    {
    }

    public record ProfileByUsernameQuery(string UserLoginId) : IQuery<PersonProfile>
    {
    }

    public class RetrievePersonProfileByUserLogin : IQueryHandler<ProfileByUsernameQuery, PersonProfile>
    {
        private readonly IPersonDbRepository _personDbRepository;

        public RetrievePersonProfileByUserLogin(IPersonDbRepository personDbRepository)
        {
            _personDbRepository = personDbRepository;
        }

        public async Task<PersonProfile> HandleAsync(ProfileByUsernameQuery query, CancellationToken ct = default)
        {
            var person = new PersonViewModel(await _personDbRepository.ProfileByUserLoginId(query.UserLoginId));

            return new PersonProfile(person);
        }
    }
}
