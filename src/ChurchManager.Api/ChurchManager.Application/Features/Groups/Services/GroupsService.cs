using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;

namespace ChurchManager.Application.Features.Groups.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupDbRepository2 _groupDb;

        public GroupsService(IGroupDbRepository2 groupDb)
        {
            _groupDb = groupDb;
        }

        public async Task<IEnumerable<GroupTypeRole>> GroupRolesForGroupAsync(int groupId, CancellationToken ct = default)
        {
            var groupTypeSpec = new GroupTypeForGroupSpecification(groupId);
            var groupType = await _groupDb.GetBySpecAsync<GroupType>(groupTypeSpec, ct);

            var groupTypeRoles = _groupDb.DbContext.Set<GroupTypeRole>()
                .Where(x => x.GroupTypeId == groupType.Id);
            return groupTypeRoles;
        }
    }
}
