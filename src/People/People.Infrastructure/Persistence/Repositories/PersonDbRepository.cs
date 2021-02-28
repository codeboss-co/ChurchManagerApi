using System.Threading.Tasks;
using ChurchManager.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using People.Domain.Repositories;
using People.Infrastructure.Persistence.Specifications;
using People.Persistence.Models;

namespace People.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : CrudDatabaseRepository<Person>, IPersonDbRepository
    {
        public PersonDbRepository(PeopleDbContext dbContext) : base(dbContext) { }

        public async Task<Person> ProfileByUserLoginId(string userLoginId)
        {
            return await Queryable(new ProfileByUserLoginSpecification(userLoginId)).FirstOrDefaultAsync();
        }
    }
}
