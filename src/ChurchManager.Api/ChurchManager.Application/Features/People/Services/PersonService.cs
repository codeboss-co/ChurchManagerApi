using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;

namespace ChurchManager.Application.Features.People.Services
{

    public interface IPersonService
    {
        Task<PersonViewModel> PersonByUserLoginId(string userLoginId, CancellationToken ct = default);
    }

    public class PersonService : IPersonService
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PersonViewModel> PersonByUserLoginId(string userLoginId, CancellationToken ct)
        {
            var entity = await _dbRepository.ProfileByUserLoginId(userLoginId, ct);

            var vm = _mapper.Map<PersonViewModel>(entity);

            return vm;
        }
    }
}
