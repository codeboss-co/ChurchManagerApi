using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using Groups.Persistence.Models;

namespace Domain.Repositories
{
    public interface IGroupDbRepository : ICrudDatabaseRepository<Group>
    {
        Task<IEnumerable<Group>> AllPersonsGroups(int personId);
    }
}
