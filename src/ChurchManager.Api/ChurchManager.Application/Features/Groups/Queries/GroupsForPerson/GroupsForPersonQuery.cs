using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.GroupsForPerson
{
    public record GroupsForPersonQuery(int PersonId) : IRequest<ApiResponse>;

    public class GroupsForPersonQueryHandler : IRequestHandler<GroupsForPersonQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IMapper _mapper;

        public GroupsForPersonQueryHandler(IGroupDbRepository groupDbRepository, IMapper mapper)
        {
            _groupDbRepository = groupDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(GroupsForPersonQuery query, CancellationToken ct)
        {
            var spec = new PersonsGroupsSpecification(query.PersonId, RecordStatus.Active);

            var groups = await _groupDbRepository.ListAsync(spec, ct);

            var viewModels = _mapper.Map<IEnumerable<GroupSummaryViewModel>>(groups);

            return new ApiResponse(new GroupSummariesViewModel(viewModels));
        }
    }
}