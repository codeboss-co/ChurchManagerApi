using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.GroupsWithChildren
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

            return new ApiResponse(groups);
        }
    }
}