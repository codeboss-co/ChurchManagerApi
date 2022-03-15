using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Communication.Services
{
    public class WebPushPushNotification : IPushNotificationService
    {
        public ILogger<WebPushPushNotification> Logger { get; }
        private readonly IPushServiceClient _client;
        private readonly IGenericDbRepository<PushDevice> _dbRepository;

        public WebPushPushNotification(
            IPushServiceClient client,
            IGenericDbRepository<PushDevice> dbRepository,
            ILogger<WebPushPushNotification> logger)
        {
            Logger = logger;
            _client = client;
            _dbRepository = dbRepository;
        }

        /*
         * Example Notification
         *
         *{
                "notification":{
                    "title":"Web Mail Notification",
                    "body":"New Mail Received!",
                    "icon":"images/bell.jpg",
                    "vibrate":[100, 50, 100 ],
                    "requireInteraction":true,
                    "data":{
                        "dateOfArrival":1620921655995
                    },
                    "actions":[
                       {
                           "action":"inbox",
                           "title":"Go to Web Mail"
                       }
                    ]
                }
           }
         *
         */
        public async Task SendNotificationToPersonAsync(int personId, PushNotification notification,
            CancellationToken ct = default)
        {
            // Get person devices
            var devices = await _dbRepository
                .Queryable()
                .AsNoTracking()
                .Where(x => x.PersonId == personId)
                .ToListAsync(ct);

            Logger.LogInformation($"Devices found for {personId} : {devices.Count}");

            //var payload =
            //    "{\"notification\":{\"title\":\"Web Mail Notification\",\"body\":\"New Mail Received!\",\"icon\":\"images/bell.jpg\",\"vibrate\":[100,50,100],\"requireInteraction\":true,\"data\":{\"dateOfArrival\":1620921655995},\"actions\":[{\"action\":\"inbox\",\"title\":\"Go to Web Mail\"}]}}";

            foreach (var device in devices) await _client.SendNotificationAsync(device, notification, ct);

            Logger.LogInformation($"Done sending web push notifications to {personId}.");
        }
    }
}