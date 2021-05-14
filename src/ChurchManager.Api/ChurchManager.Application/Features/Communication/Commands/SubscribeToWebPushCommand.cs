using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Domain;
using Lib.Net.Http.WebPush;
using MediatR;

namespace ChurchManager.Application.Features.Communication.Commands
{
    public record SubscribeToWebPushCommand(PushSubscription Subscription) : IRequest
    {
    }

    public record UnsubscribeToWebPushCommand(PushSubscription Subscription) : IRequest
    {
    }

    public class WebPushSubscriptionHandler : IRequestHandler<SubscribeToWebPushCommand>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IPushSubscriptionsService _push;

        public WebPushSubscriptionHandler(ICognitoCurrentUser currentUser, IPushSubscriptionsService push)
        {
            _currentUser = currentUser;
            _push = push;
        }

        public async Task<Unit> Handle(SubscribeToWebPushCommand command, CancellationToken ct)
        {
            await _push.SubscribeAsync("test", command.Subscription, _currentUser.PersonId, ct);
            
            return new Unit();
        }
    }

    public class WebPushUnsubscribeHandler : IRequestHandler<UnsubscribeToWebPushCommand>
    {
        private readonly IPushSubscriptionsService _push;

        public WebPushUnsubscribeHandler(IPushSubscriptionsService push)
        {
            _push = push;
        }

        public async Task<Unit> Handle(UnsubscribeToWebPushCommand command, CancellationToken ct)
        {
            await _push.UnsubscribeAsync(command.Subscription, ct);

            return new Unit();
        }
    }
}
