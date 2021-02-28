using System.Threading.Tasks;
using People.Contracts;
using People.Domain.Model;
using People.Domain.Repositories;

namespace People.Application.Services
{
    public interface IPersonApplicationService
    {
        Task<PersonDomain> PersonByUserLoginId(string userLoginId);
    }

    public class PersonApplicationService : IPersonApplicationService
    {
        private readonly IPersonDbRepository _dbRepository;

        public PersonApplicationService(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PersonDomain> PersonByUserLoginId(string userLoginId)
        {
            return new (await _dbRepository.ProfileByUserLoginId(userLoginId));
        }
    }
}
