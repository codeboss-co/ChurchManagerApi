using System.Linq;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupMemberDbRepository : IGenericDbRepository<GroupMember>
    {
        IQueryable<GroupMember> GetByGroupId(int groupId);
        IQueryable<GroupMember> GetLeaders(int groupId);
        Task<(int peopleCount, int leadersCount)> PeopleAndLeadersInGroupsAsync(int groupTypeId);
    }
}
