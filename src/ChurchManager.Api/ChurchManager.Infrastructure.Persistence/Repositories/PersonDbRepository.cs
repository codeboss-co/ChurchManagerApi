using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryBase<Person>, IPersonDbRepository
    {
        public PersonDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
