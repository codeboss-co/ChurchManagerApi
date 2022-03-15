using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.GroupsWithChildren
{
    public record GroupWithChildrenQuery(int GroupId) : IRequest<ApiResponse>
    {
    }

    public class GroupWithChildrenHandler : IRequestHandler<GroupWithChildrenQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;

        public GroupWithChildrenHandler(IGroupDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(GroupWithChildrenQuery query, CancellationToken ct)
        {
            var groups = await _dbRepository.GroupWithChildrenAsync(query.GroupId, maxDepth:2, ct: ct);

            // Ordering
            groups = groups.OrderBy(x => x.Name);
            // Order the group children
            groups.ForEach(x => x.Groups = x.Groups.OrderBy(x => x.Name));

            return new ApiResponse(groups);
        }
    }
}