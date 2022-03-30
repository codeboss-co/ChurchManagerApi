using ChurchManager.Domain.Features.Groups;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IGroupsService
    {
        Task<IEnumerable<GroupTypeRole>> GroupRolesForGroupAsync(int groupId, CancellationToken ct = default);
    }
}
