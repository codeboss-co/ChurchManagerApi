using System;
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
        Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int? parentGroupId = null, int maxDepth = 10, CancellationToken ct = default);
        Task<IEnumerable<GroupViewModel>> GroupWithChildrenAsync(int groupId, int maxDepth = 10, CancellationToken ct = default);

        Task<(int totalGroupsCount, int activeGroupsCount, int inActiveGroupsCount, int onlineGroupsCount, int openedGroupsCount, int closedGroupsCount)> GroupStatisticsAsync(int groupTypeId, DateTime? startDate = null, CancellationToken ct = default);
    }
}
