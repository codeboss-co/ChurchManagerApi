using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Persistence.Contexts;

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

        public async Task SubscribeAsync(string deviceType, PushSubscription subscription, int personId, CancellationToken ct = default)
        {
            var device = new PushDevice
            {
                Name = deviceType,
                PersonId = personId,
                Endpoint = subscription.Endpoint,
                P256DH = subscription.GetKey(PushEncryptionKeyName.P256DH),
                Auth = subscription.GetKey(PushEncryptionKeyName.Auth)
            };

            await AddAsync(device, ct);
        }

        public async Task UnsubscribeAsync(PushSubscription subscription,  CancellationToken ct = default)
        {
            var device = Queryable().First(x => x.Endpoint == subscription.Endpoint);
            await DeleteAsync(device, ct);
        }
    }
}
