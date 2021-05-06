using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;

namespace ChurchManager.Application.Features.People.Services
{

    public interface IPersonService
    {
        Task<PersonDomain> PersonByUserLoginId(string userLoginId);
    }

    public class PersonService : IPersonService
    {
        private readonly IPersonDbRepository _dbRepository;

        public PersonService(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public Task<PersonDomain> PersonByUserLoginId(string userLoginId) 
            => _dbRepository.ProfileByUserLoginId(userLoginId);
    }
}
