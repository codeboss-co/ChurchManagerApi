using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using People.Persistence.Models;

namespace People.Domain.Repositories
{
    public interface IPersonDbRepository : ICrudDatabaseRepository<Person>
    {
        Task<Person> ProfileByUserLoginId(string userLoginId);
    }
}
