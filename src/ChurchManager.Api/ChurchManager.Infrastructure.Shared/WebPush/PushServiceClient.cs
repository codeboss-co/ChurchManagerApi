using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using WebPush;
using PushSubscription = WebPush.PushSubscription;

namespace ChurchManager.Infrastructure.Shared.WebPush
{
    public class PushServiceClient : IPushServiceClient
    {
        private readonly WebPushClient _client;

        public PushServiceClient(WebPushClient client) => _client = client;

        public async Task SendNotificationAsync(PushDevice device, string payload, CancellationToken ct = default)
        {
            var subscription = new PushSubscription(device.Endpoint, device.P256DH, device.Auth);

            await _client.SendNotificationAsync(subscription, payload);
        }
    }
}
