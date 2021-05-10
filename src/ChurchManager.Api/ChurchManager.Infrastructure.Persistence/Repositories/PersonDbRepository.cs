using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryBase<Person>, IPersonDbRepository
    {
        public PersonDbRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
