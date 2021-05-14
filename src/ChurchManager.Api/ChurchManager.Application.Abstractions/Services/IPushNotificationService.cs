using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IPushNotificationService
    {
        Task SendNotificationToPersonAsync(int personId, string payload, CancellationToken cancellationToken = default);
    }
}
