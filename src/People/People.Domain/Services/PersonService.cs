using System.Threading.Tasks;
using People.Domain.Model;
using People.Infrastructure.Persistence.Repositories;

namespace People.Domain.Services
{
    public class PersonService
    {
        private readonly IPersonDbRepository _dbRepository;

        public PersonService(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PersonDomain> PersonByUserLoginId(string userLoginId)
        {
            return new (await _dbRepository.PersonByUserLoginId(userLoginId));
        }
    }
}
