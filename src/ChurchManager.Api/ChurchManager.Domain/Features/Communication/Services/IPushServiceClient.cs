using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IPushServiceClient
    {
        Task SendNotificationAsync(PushDevice device, string payload, CancellationToken cancellationToken = default);
    }
}
