using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupMemberDbRepository : IGenericRepositoryAsync<GroupMember>
    {
        
    }
}
