using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository : IGenericDbRepository<Person>
    {
        
    }
}
