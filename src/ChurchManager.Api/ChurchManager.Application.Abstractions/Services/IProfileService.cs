using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.ViewModels;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IProfileService
    {
        Task<PersonViewModel> ProfileByUserLoginId(string userLoginId, CancellationToken ct = default);
        Task<PersonViewModel> ProfileByPersonId(int personId, bool condensed, CancellationToken ct);
    }
}