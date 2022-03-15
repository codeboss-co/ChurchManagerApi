using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.SharedKernel.Wrappers;
using Convey.CQRS.Queries;
using MediatR;

namespace ChurchManager.Features.People.Queries.BrowsePeople
{
    public record BrowsePeopleQuery : PeopleAdvancedSearchQuery, IRequest<PagedResponse<PersonViewModel>>
    {
    }

    public class BrowsePeopleHandler : IRequestHandler<BrowsePeopleQuery, PagedResponse<PersonViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _service;

        public BrowsePeopleHandler(IPersonService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PersonViewModel>> Handle(BrowsePeopleQuery query, CancellationToken ct)
        {
            var pagedResult = await _service.BrowseAsync(query, ct);

            var vm = _mapper.Map<PagedResult<PersonViewModel>>(pagedResult);

            return new PagedResponse<PersonViewModel>(vm);
        }
    }
}