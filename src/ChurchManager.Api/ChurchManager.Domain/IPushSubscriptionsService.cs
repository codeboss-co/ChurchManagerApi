using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;

namespace ChurchManager.Domain
{
    public interface IPushSubscriptionsService
    {
        IEnumerable<PushSubscription> All();
        Task SubscribeAsync(string deviceType, PushSubscription subscription, int personId, CancellationToken ct = default);
        Task UnsubscribeAsync(PushSubscription subscription, CancellationToken ct = default);
    }
}
