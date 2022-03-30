using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.GroupRoles
{
    public record GroupRolesQuery(int GroupTypeId) : IRequest<ApiResponse>
    {
    }

    public class GroupRolesQueryHandler : IRequestHandler<GroupRolesQuery, ApiResponse>
    {
        private readonly IGroupTypeRoleDbRepository _dbRepository;

        public GroupRolesQueryHandler(IGroupTypeRoleDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(GroupRolesQuery query, CancellationToken ct)
        {
            var roles = await _dbRepository
                .GetByGroupTypeId(query.GroupTypeId)
                .ToListAsync(ct);

            return new ApiResponse(roles);
        }
    }
}