using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;

namespace ChurchManager.Domain
{
    public interface IPushServiceClient
    {
        Task SendNotificationAsync(PushDevice device, string payload, CancellationToken cancellationToken = default);
    }
}
