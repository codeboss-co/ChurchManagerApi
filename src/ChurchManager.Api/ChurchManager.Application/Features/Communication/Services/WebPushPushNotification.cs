using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Shared.WebPush;
using Microsoft.Extensions.Options;
using WebPush;
using PushSubscription = WebPush.PushSubscription;

namespace ChurchManager.Application.Features.Communication.Services
{
    public class WebPushPushNotification : IPushNotificationService
    {
        private readonly PushNotificationsOptions _options;

        public WebPushPushNotification(IOptions<PushNotificationsOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendNotificationAsync(PushDevice device, string payload, CancellationToken ct = default)
        {
            var subscription = new PushSubscription(device.Endpoint, device.P256DH, device.Auth);
            var vapidDetails = new VapidDetails("mailto:example@example.com", _options.PublicKey, _options.PrivateKey);

            payload =
                "{\"notification\":{\"title\":\"Web Mail Notification\",\"body\":\"New Mail Received!\",\"icon\":\"images/bell.jpg\",\"vibrate\":[100,50,100],\"requireInteraction\":true,\"data\":{\"dateOfArrival\":1620921655995},\"actions\":[{\"action\":\"inbox\",\"title\":\"Go to Web Mail\"}]}}";

            var webPushClient = new WebPushClient();

            await webPushClient.SendNotificationAsync(subscription, payload, vapidDetails);
        }
    }
}
