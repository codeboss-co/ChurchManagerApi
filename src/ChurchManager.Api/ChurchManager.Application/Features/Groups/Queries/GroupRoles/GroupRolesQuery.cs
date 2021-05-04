using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.GroupRoles
{
    public record GroupRolesQuery(int GroupId) : IRequest<ApiResponse>
    {
    }

    public class GroupRolesQueryHandler : IRequestHandler<GroupRolesQuery, ApiResponse>
    {
        private readonly IGroupDbRepository _groupDbRepository;

        public GroupRolesQueryHandler(IGroupDbRepository groupDbRepository)
        {
            _groupDbRepository = groupDbRepository;
        }

        public async Task<ApiResponse> Handle(GroupRolesQuery query, CancellationToken ct)
        {
            var roles = await _groupDbRepository.GroupRolesForGroupAsync(query.GroupId, ct);

            return new ApiResponse(roles);
        }
    }
}
