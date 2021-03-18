using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Groups;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupDbRepository : IGenericRepositoryAsync<Group>
    {
        Task<IEnumerable<GroupDomain>> AllPersonsGroups(int personId, RecordStatus recordStatus, CancellationToken ct = default);
        Task<PagedResult<GroupDomain>> BrowsePersonsGroups(int personId, string search, QueryParameter query, CancellationToken ct = default);
        Task<IEnumerable<GroupMemberViewModel>> GroupMembersAsync(int groupId, CancellationToken ct = default);
        Task<IEnumerable<GroupMemberRole>> GroupRolesForGroupAsync(int groupId, CancellationToken ct);
    }
}
