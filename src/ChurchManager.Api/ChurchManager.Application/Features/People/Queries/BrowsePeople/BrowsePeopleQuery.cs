using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Parameters;
using MediatR;

namespace ChurchManager.Application.Features.People.Queries.BrowsePeople
{
    public record BrowsePeopleQuery : SearchTermQueryParameter, IRequest<PagedResponse<PersonViewModel>>
    {
    }

    public class BrowsePeopleHandler : IRequestHandler<BrowsePeopleQuery, PagedResponse<PersonViewModel>>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public BrowsePeopleHandler(IPersonDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PersonViewModel>> Handle(BrowsePeopleQuery query, CancellationToken ct)
        {
            var spec = new BrowsePeopleSpecification(query);
            var pagedResult = await _dbRepository.BrowseAsync(query, spec, ct);

            try
            {
                var vm = _mapper.Map<Convey.CQRS.Queries.PagedResult<PersonViewModel>>(pagedResult);

                return new PagedResponse<PersonViewModel>(vm);

                return null;
            }
            catch(System.Exception ex)
            {

                throw;
            }
        }
    }
}
