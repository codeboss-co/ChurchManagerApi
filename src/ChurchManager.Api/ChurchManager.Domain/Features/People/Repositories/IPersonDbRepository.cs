using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Model;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository
    {
        Task<PersonDomain> ProfileByUserLoginId(string userLoginId);
        Task<UserDetails> UserDetailsByUserLoginId(string queryUserLoginId);
    }
}
