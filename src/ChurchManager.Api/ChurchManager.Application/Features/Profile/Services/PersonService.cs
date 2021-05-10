using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Parameters;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Features.Profile.Services
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

        public async Task<PagedResult<PersonViewModel>> BrowseAsync(SearchTermQueryParameter query, CancellationToken ct = default)
        {
            var spec = new BrowsePeopleSpecification(query);

            var pagedResult = await _dbRepository.BrowseAsync(query, spec, ct);

            var vm = _mapper.Map<PagedResult<PersonViewModel>>(pagedResult);

            return vm;
        }
    }
}
