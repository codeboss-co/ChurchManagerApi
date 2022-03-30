using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PushSubscriptionsService : GenericRepositoryBase<PushDevice>, IPushSubscriptionsService
    {
        public PushSubscriptionsService(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<PushSubscription> All()
        {
            throw new NotImplementedException();
        }

        public async Task SubscribeAsync(PushSubscription subscription, string deviceType, string uniqueIdentification, int personId, CancellationToken ct = default)
        {
            _ = subscription ?? throw new ArgumentNullException(nameof(subscription));

            var p256dh = subscription.GetKey(PushEncryptionKeyName.P256DH);
            if (!await Queryable().AnyAsync(s => s.P256DH == p256dh, ct))
            {
                var device = new PushDevice
                {
                    Name = deviceType,
                    PersonId = personId,
                    Endpoint = subscription.Endpoint,
                    P256DH = subscription.GetKey(PushEncryptionKeyName.P256DH),
                    Auth = subscription.GetKey(PushEncryptionKeyName.Auth),
                    UniqueIdentification = uniqueIdentification
                };

                await AddAsync(device, ct);
            }
        }

        public async Task UnsubscribeAsync(PushSubscription subscription,  CancellationToken ct = default)
        {
            _ = subscription ?? throw new ArgumentNullException(nameof(subscription));

            var p256dh = subscription.GetKey(PushEncryptionKeyName.P256DH);
            if (await Queryable().AnyAsync(s => s.P256DH == p256dh, ct))
            {
                var device = Queryable().First(x => x.Endpoint == subscription.Endpoint);
                await DeleteAsync(device, ct);
            }
        }

        public async Task UnsubscribeAsync(string deviceType, string uniqueIdentification, int personId, CancellationToken ct = default)
        {
            var device = Queryable().FirstOrDefault(s => 
                s.Name == deviceType &&
                s.UniqueIdentification == uniqueIdentification &&
                s.PersonId == personId);

            if(device is not null)
            {
                await DeleteAsync(device, ct);
            }
        }
    }
}
