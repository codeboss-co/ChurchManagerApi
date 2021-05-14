using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Communication.Services
{
    public class WebPushPushNotification : IPushNotificationService
    {
        private readonly IPushServiceClient _client;
        private readonly IGenericDbRepository<PushDevice> _dbRepository;

        public WebPushPushNotification(IPushServiceClient client, IGenericDbRepository<PushDevice> dbRepository)
        {
            _client = client;
            _dbRepository = dbRepository;
        }

        public async Task SendNotificationToPersonAsync(int personId, string payload, CancellationToken ct = default)
        {
            // Get person devices
            var devices = await _dbRepository.Queryable()
                .Where(x => x.PersonId == personId)
                .ToListAsync(ct);

            payload =
                "{\"notification\":{\"title\":\"Web Mail Notification\",\"body\":\"New Mail Received!\",\"icon\":\"images/bell.jpg\",\"vibrate\":[100,50,100],\"requireInteraction\":true,\"data\":{\"dateOfArrival\":1620921655995},\"actions\":[{\"action\":\"inbox\",\"title\":\"Go to Web Mail\"}]}}";

            foreach (var device in devices)
            {
                await _client.SendNotificationAsync(device, payload, ct);
            }
        }
    }
}
