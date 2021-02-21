using System.Threading.Tasks;
using People.Domain;
using People.Domain.Model;
using People.Domain.Repositories;
using People.Domain.Services;

namespace People.Application.Services
{
    public class PersonApplicationService : IPersonApplicationService
    {
        private readonly IPersonDbRepository _dbRepository;

        public PersonApplicationService(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PersonDomain> PersonByUserLoginId(string userLoginId)
        {
            return new (await _dbRepository.PersonByUserLoginId(userLoginId));
        }
    }
}
