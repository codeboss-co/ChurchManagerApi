using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Commands.AddGroupMember
{
    public record UpdateGroupMemberCommand : IRequest<ApiResponse>
    {
        public int GroupMemberId { get; set; }
        public int PersonId { get; set; }
        public int GroupRoleId { get; set; }
        public string CommunicationPreference { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public string RecordStatus { get; set; }
    }

    public class GroupMemberUpdatedHandler : IRequestHandler<UpdateGroupMemberCommand, ApiResponse>
    {
        private readonly IGenericDbRepository<GroupMember> _dbRepository;

        public GroupMemberUpdatedHandler(IGenericDbRepository<GroupMember> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(UpdateGroupMemberCommand command, CancellationToken ct)
        {
            var groupMember = await _dbRepository.GetByIdAsync(command.GroupMemberId, ct);

            if (groupMember is not null)
            {
                groupMember.PersonId = command.PersonId;
                groupMember.CommunicationPreference = command.CommunicationPreference;
                groupMember.GroupRoleId = command.GroupRoleId;
                groupMember.FirstVisitDate = command.FirstVisitDate;
                groupMember.RecordStatus = command.RecordStatus;

                await _dbRepository.SaveChangesAsync(ct);

                return new ApiResponse(true);
            }

            return new ApiResponse(false);
        }
    }
}