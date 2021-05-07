using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;

namespace ChurchManager.Application.Features.Profile.Services
{

    public interface IProfileService
    {
        Task<PersonViewModel> ProfileByUserLoginId(string userLoginId, CancellationToken ct = default);
        Task<PersonViewModel> ProfileByPersonId(int personId, bool condensed, CancellationToken ct);
    }

    public class ProfileService : IProfileService
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public ProfileService(IPersonDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PersonViewModel> ProfileByUserLoginId(string userLoginId, CancellationToken ct)
        {
            var entity = await _dbRepository.ProfileByUserLoginId(userLoginId, ct);

            var vm = _mapper.Map<PersonViewModel>(entity);

            return vm;
        }

        public async Task<PersonViewModel> ProfileByPersonId(int personId, bool condensed, CancellationToken ct)
        {
            var entity = await _dbRepository.ProfileByPersonId(personId, condensed, ct);

            var vm = _mapper.Map<PersonViewModel>(entity);

            return vm;
        }
    }
}
