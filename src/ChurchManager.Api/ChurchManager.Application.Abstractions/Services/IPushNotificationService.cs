using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Communication;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IPushNotificationService
    {
        Task SendNotificationToPersonAsync(int personId, PushNotification notification, CancellationToken cancellationToken = default);
    }
}
