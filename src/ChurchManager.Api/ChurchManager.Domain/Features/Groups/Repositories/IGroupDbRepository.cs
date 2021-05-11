using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupDbRepository : IGenericDbRepository<Group>
    {
        Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, RecordStatus status,CancellationToken ct = default);
        Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int maxDepth, CancellationToken ct = default);
    }
}
