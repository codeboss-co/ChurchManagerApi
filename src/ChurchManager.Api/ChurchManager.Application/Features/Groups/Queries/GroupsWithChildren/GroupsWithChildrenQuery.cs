using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using CodeBoss.Extensions;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.GroupsWithChildren
{
    public record GroupsWithChildrenQuery : IRequest<ApiResponse>
    {
        public int? ParentGroupId { get; set; } = null;
    }

    public class GroupsWithChildrenHandler : IRequestHandler<GroupsWithChildrenQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public GroupsWithChildrenHandler(IGroupDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }


        public async Task<ApiResponse> Handle(GroupsWithChildrenQuery request, CancellationToken ct)
        {
            var groups = await _dbRepository.GroupsWithChildrenAsync(maxDepth:2, ct: ct);

            // Ordering
            groups = groups.OrderBy(x => x.Name);
            // Order the group children
            groups.ForEach(x => x.Groups = x.Groups.OrderBy(x => x.Name));

            return new ApiResponse(groups);
        }
    }
}