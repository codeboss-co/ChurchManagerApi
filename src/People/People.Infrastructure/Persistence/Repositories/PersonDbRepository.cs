using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using People.Domain;
using People.Domain.Repositories;
using People.Infrastructure.Persistence.Specifications;
using People.Persistence.Models;

namespace People.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : CrudDatabaseRepository<Person>, IPersonDbRepository
    {
        public PersonDbRepository(PeopleDbContext dbDbContext) : base(dbDbContext) { }

        public async Task<Person> PersonByUserLoginId(string userLoginId)
        {
            return await Queryable(new PersonByUserLoginSpecification(userLoginId)).SingleAsync();
        }
    }
}
