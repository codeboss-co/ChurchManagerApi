using System.Linq;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupMemberDbRepository : IGenericDbRepository<GroupMember>
    {
        IQueryable<GroupMember> GetByGroupId(int groupId);
        IQueryable<GroupMember> GetLeaders(int groupId);
    }
}
