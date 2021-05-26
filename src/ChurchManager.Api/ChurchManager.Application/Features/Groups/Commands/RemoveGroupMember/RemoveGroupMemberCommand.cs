using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Commands.RemoveGroupMember
{
    public record RemoveGroupMemberCommand(int GroupId, int GroupMemberId) : IRequest<ApiResponse>
    {
    }

    public class GroupMemberRemovedHandler : IRequestHandler<RemoveGroupMemberCommand, ApiResponse>
    {
        private readonly IGenericDbRepository<GroupMember> _dbRepository;

        public GroupMemberRemovedHandler(IGenericDbRepository<GroupMember> dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<ApiResponse> Handle(RemoveGroupMemberCommand command, CancellationToken ct)
        {
            // Check they are not a group member already
            var groupMember =  await _dbRepository
                .Queryable()
                .FirstOrDefaultAsync(x => 
                    x.Id == command.GroupMemberId &&
                    x.GroupId == command.GroupId, ct);

            if (groupMember is not null)
            {
                await _dbRepository.DeleteAsync(groupMember, ct);
            }

            return new ApiResponse(true);
        }
    }

}
