using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.GroupMembers
{
    public record GroupMemberQuery(int GroupMemberId) : IRequest<ApiResponse>
    {
    }

    public class GroupMemberHandler : IRequestHandler<GroupMemberQuery, ApiResponse>
    {
        private readonly IGroupMemberDbRepository _dbRepository;

        public GroupMemberHandler(IGroupMemberDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(GroupMemberQuery query, CancellationToken ct)
        {
            var spec = new GroupMemberSpecification(query.GroupMemberId);

            var member = await _dbRepository.GetBySpecAsync<GroupMemberEditViewModel>(spec, ct);

            return new ApiResponse(member);
        }
    }
}