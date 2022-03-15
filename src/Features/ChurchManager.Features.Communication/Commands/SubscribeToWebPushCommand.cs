using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.SharedKernel.Common;
using MediatR;

namespace ChurchManager.Features.Communication.Commands
{
    public record SubscribeToWebPushCommand(PushSubscription Subscription) : IRequest
    {
        public string Device { get; set; }
        public string UniqueIdentification { get; set; }
    }

    public record UnsubscribeToWebPushCommand(PushSubscription Subscription) : IRequest;

    public record RemoveSubscriptionsToWebPushCommand(string Device, string UniqueIdentification, int PersonId) : IRequest;


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
            await _push.SubscribeAsync(command.Subscription, command.Device, command.UniqueIdentification, _currentUser.PersonId, ct);

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

    public class WebPushRemoveSubscriptionHandler : IRequestHandler<RemoveSubscriptionsToWebPushCommand>
    {
        private readonly IPushSubscriptionsService _push;

        public WebPushRemoveSubscriptionHandler(IPushSubscriptionsService push)
        {
            _push = push;
        }

        public async Task<Unit> Handle(RemoveSubscriptionsToWebPushCommand command, CancellationToken ct)
        {
            await _push.UnsubscribeAsync(command.Device, command.UniqueIdentification, command.PersonId, ct);

            return new Unit();
        }
    }
}