using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using People.Infrastructure.Persistence.Model;

namespace People.Infrastructure.Persistence.Repositories
{
    public interface IPersonDbRepository : ICrudDatabaseRepository<Person>
    {
        Task<Person> PersonByUserLoginId(string userLoginId);
    }
}
