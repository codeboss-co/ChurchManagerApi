using Codeboss.Types;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Domain.Features.Communication.Services
{
    // Marker for easy access
    public interface IPushNotificationsService
    {
        Task PushAsync(INotification notification, CancellationToken token = default);
    }
}
