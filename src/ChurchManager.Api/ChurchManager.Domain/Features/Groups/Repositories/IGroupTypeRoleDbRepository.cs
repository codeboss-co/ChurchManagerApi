using System.Linq;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Groups;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupTypeRoleDbRepository: IGenericRepositoryAsync<GroupTypeRole>
    {
        IQueryable<GroupTypeRole> GetByGroupTypeId(int groupTypeId);
    }
}
