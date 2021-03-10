using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups
{
    public record BrowsePersonsGroupsQuery(string SearchTerm, int PersonId)
        : QueryParameter, IRequest<PagedResponse<DomainEntity>> { }

    public class BrowsePersonsGroupsHandler : IRequestHandler<BrowsePersonsGroupsQuery, PagedResponse<DomainEntity>>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IModelHelper _modelHelper;

        public BrowsePersonsGroupsHandler(ICognitoCurrentUser currentUser, IGroupDbRepository groupDbRepository, IModelHelper modelHelper)
        {
            _currentUser = currentUser;
            _groupDbRepository = groupDbRepository;
            _modelHelper = modelHelper;
        }

        public async Task<PagedResponse<DomainEntity>> Handle(BrowsePersonsGroupsQuery query, CancellationToken ct)
        {
            var validFilter = query;
            //filtered fields security
            if(!string.IsNullOrEmpty(validFilter.Fields))
            {
                //limit to fields in view model
                validFilter.Fields = _modelHelper.ValidateModelFields<GroupViewModel>(validFilter.Fields);
            }
            if(string.IsNullOrEmpty(validFilter.Fields))
            {
                //default fields from view model
                validFilter.Fields = _modelHelper.GetModelFields<GroupViewModel>();
            }

            var pagedResult = await _groupDbRepository.BrowsePersonsGroups(query.PersonId, query.SearchTerm, query, ct);

            return new PagedResponse<DomainEntity>(pagedResult);
        }
    }
}
