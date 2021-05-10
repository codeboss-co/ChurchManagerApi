using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupDbRepository2 : IGenericDbRepository<Group>
    {
        Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, RecordStatus status,CancellationToken ct = default);
    }
}
