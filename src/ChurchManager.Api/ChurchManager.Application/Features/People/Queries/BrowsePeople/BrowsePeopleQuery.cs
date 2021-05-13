using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Parameters;
using MediatR;

namespace ChurchManager.Application.Features.People.Queries.BrowsePeople
{
    public record BrowsePeopleQuery : SearchTermQueryParameter, IRequest<PagedResponse<PersonViewModel>>
    {
    }

    public class BrowsePeopleHandler : IRequestHandler<BrowsePeopleQuery, PagedResponse<PersonViewModel>>
    {
        private readonly IPersonService _service;
        private readonly IMapper _mapper;

        public BrowsePeopleHandler(IPersonService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PersonViewModel>> Handle(BrowsePeopleQuery query, CancellationToken ct)
        {
            var pagedResult = await _service.BrowseAsync(query, ct);

            try
            {
                var vm = _mapper.Map<Convey.CQRS.Queries.PagedResult<PersonViewModel>>(pagedResult);

                return new PagedResponse<PersonViewModel>(vm);
            }
            catch(System.Exception ex)
            {

                throw;
            }
        }
    }
}
