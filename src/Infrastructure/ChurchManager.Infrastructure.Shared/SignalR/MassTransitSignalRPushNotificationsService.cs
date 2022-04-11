using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using Codeboss.Types;
using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class MassTransitSignalRPushNotificationsService : IPushNotificationsService
    {
        private readonly IBusControl _busControl;
        private readonly IReadOnlyList<IHubProtocol> _protocols = new IHubProtocol[] { new JsonHubProtocol() };

        public MassTransitSignalRPushNotificationsService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task PushAsync(INotification notification, CancellationToken token = default)
        {
            switch (notification.Scope)
            {
                case Constants.Notifications.Scope.All:
                {
                    await _busControl.Publish<All<NotificationHub>>(new
                        {
                            Messages = _protocols.ToProtocolDictionary(notification.MethodName, new object[] { notification })
                        }, token)
                        .ConfigureAwait(false);
                    break;
                }
                case Constants.Notifications.Scope.User:
                {
                    await _busControl.Publish<User<NotificationHub>>(new
                        {
                            notification.UserId,
                            Messages = _protocols.ToProtocolDictionary(notification.MethodName, new object[] { notification })
                        }, token)
                        .ConfigureAwait(false);
                    break;
                }
            }
        }
    }
}