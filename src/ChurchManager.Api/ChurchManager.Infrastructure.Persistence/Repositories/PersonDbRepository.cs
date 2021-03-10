using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Model;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Specifications;
using ChurchManager.Persistence.Models.People;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryAsync<Person>, IPersonDbRepository
    {
        private readonly IDataShapeHelper<Person> _dataShaper;

        public PersonDbRepository(
            ChurchManagerDbContext dbContext,
            IDataShapeHelper<Person> dataShaper) : base(dbContext)
        {
            _dataShaper = dataShaper;
        }

        public async Task<PersonDomain> ProfileByUserLoginId(string userLoginId)
        {
            var entity = await Queryable(new ProfileByUserLoginSpecification(userLoginId)).FirstOrDefaultAsync();

            return entity is not null
                ? new PersonDomain(entity)
                : null;
        }
    }
}
