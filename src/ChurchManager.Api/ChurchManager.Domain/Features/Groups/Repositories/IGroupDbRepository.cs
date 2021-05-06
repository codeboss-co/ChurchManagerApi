using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Model;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupDbRepository : IGenericRepositoryAsync<Group>
    {
        Task<IEnumerable<GroupDomain>> AllPersonsGroups(int personId, RecordStatus recordStatus, CancellationToken ct = default);
        Task<PagedResult<GroupDomain>> BrowsePersonsGroups(int personId, string search, QueryParameter query, CancellationToken ct = default);
        Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, CancellationToken ct = default);
        Task<IEnumerable<GroupTypeRole>> GroupRolesForGroupAsync(int groupId, CancellationToken ct = default);
        Task<IEnumerable<GroupViewModel>> GroupsWithChildrenAsync(int maxDepth, CancellationToken ct = default);
    }
}
