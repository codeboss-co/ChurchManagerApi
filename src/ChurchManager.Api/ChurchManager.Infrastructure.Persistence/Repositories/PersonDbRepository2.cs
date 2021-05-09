using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository2 : GenericRepositoryBase<Person>, IPersonDbRepository2
    {
        public PersonDbRepository2(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
