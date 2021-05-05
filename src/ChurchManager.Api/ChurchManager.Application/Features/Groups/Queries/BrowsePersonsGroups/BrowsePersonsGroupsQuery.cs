using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Infrastructure.Abstractions;
using Convey.CQRS.Queries;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups
{
    public record BrowsePersonsGroupsQuery
        : SearchTermQueryParameter, IRequest<PagedResponse<GroupSummaryViewModel>>
    {
        public int PersonId { get; set; }
    }

    public class BrowsePersonsGroupsHandler : IRequestHandler<BrowsePersonsGroupsQuery, PagedResponse<GroupSummaryViewModel>>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IModelHelper _modelHelper;
        private readonly IMapper _mapper;

        public BrowsePersonsGroupsHandler(
            ICognitoCurrentUser currentUser,
            IGroupDbRepository groupDbRepository,
            IModelHelper modelHelper,
            IMapper mapper)
        {
            _currentUser = currentUser;
            _groupDbRepository = groupDbRepository;
            _modelHelper = modelHelper;
            _mapper = mapper;
        }

        public async Task<PagedResponse<GroupSummaryViewModel>> Handle(BrowsePersonsGroupsQuery query, CancellationToken ct)
        {
            var pagedResult = await _groupDbRepository.BrowsePersonsGroups(query.PersonId, query.SearchTerm, query, ct);

            var viewModels = _mapper.Map<PagedResult<GroupSummaryViewModel>>(pagedResult);

            return new PagedResponse<GroupSummaryViewModel>(viewModels);
        }
    }
}
