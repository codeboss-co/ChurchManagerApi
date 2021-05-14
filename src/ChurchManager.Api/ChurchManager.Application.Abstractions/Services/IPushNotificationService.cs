using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;

namespace ChurchManager.Application.Abstractions.Services
{
    public interface IPushNotificationService
    {
        Task SendNotificationAsync(PushDevice device, string payload, CancellationToken cancellationToken = default);
    }
}
