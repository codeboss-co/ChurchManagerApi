using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Commands.AddGroupMember
{
    public record AddGroupMemberCommand : IRequest<ApiResponse>
    {
        public int GroupId { get; set; }
        public int PersonId { get; set; }
        public int GroupRoleId { get; set; }
        public string CommunicationPreference { get; set; }
        public DateTime? FirstVisitDate { get; set; }
    }

    public class GroupMemberAddedHandler : IRequestHandler<AddGroupMemberCommand, ApiResponse>
    {
        private readonly IGenericDbRepository<GroupMember> _dbRepository;

        public GroupMemberAddedHandler(IGenericDbRepository<GroupMember> dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<ApiResponse> Handle(AddGroupMemberCommand command, CancellationToken ct)
        {
            // Check they are not a group member already
            if (await _dbRepository.Queryable().AnyAsync(m => 
                m.PersonId == command.PersonId && m.GroupId == command.GroupId, ct))
            {
                return new ApiResponse(false);
            }

            var groupMember = new GroupMember
            {
                PersonId = command.PersonId,
                GroupId = command.GroupId,
                GroupRoleId = command.GroupRoleId,
                FirstVisitDate = command.FirstVisitDate,
                CommunicationPreference = command.CommunicationPreference
            };

            await _dbRepository.AddAsync(groupMember, ct);
            return new ApiResponse(true);
        }
    }

}
