using System.Threading.Tasks;
using ChurchManager.Domain.Model;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository
    {
        Task<PersonDomain> ProfileByUserLoginId(string userLoginId);
    }
}
