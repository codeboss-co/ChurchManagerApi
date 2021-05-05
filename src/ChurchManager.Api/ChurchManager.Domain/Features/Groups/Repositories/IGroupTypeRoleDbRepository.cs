using System.Linq;
using ChurchManager.Domain.Model.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupTypeRoleDbRepository: IGenericRepositoryAsync<GroupTypeRole>
    {
        IQueryable<GroupTypeRole> GetByGroupTypeId(int groupTypeId);
    }
}
