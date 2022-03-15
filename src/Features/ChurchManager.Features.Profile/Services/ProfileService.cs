using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;

namespace ChurchManager.Features.Profile.Services
{
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
            var spec = new ProfileByUserLoginSpecification(userLoginId);

            var entity = await _dbRepository.GetBySpecAsync(spec, ct);

            var vm = _mapper.Map<PersonViewModel>(entity);

            return vm;
        }

        public async Task<PersonViewModel> ProfileByPersonId(int personId, bool condensed, CancellationToken ct)
        {
            var spec = new ProfileByPersonSpecification(personId, condensed);

            var entity = await _dbRepository.GetBySpecAsync(spec, ct);

            var vm = _mapper.Map<PersonViewModel>(entity);

            return vm;
        }
    }
}