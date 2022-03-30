using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using Convey.CQRS.Queries;

namespace ChurchManager.Features.People.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<PersonViewModel>> BrowseAsync(PeopleAdvancedSearchQuery query,
            CancellationToken ct = default)
        {
            var spec = new BrowsePeopleSpecification(query);

            var pagedResult = await _dbRepository.BrowseAsync(query, spec, ct);

            var vm = _mapper.Map<PagedResult<PersonViewModel>>(pagedResult);

            return vm;
        }
    }
}