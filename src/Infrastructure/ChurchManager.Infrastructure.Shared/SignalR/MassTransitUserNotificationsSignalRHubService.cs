using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class MassTransitUserNotificationsSignalRHubService :
        MassTransitSignalRHubService<NotificationHub>,
        IUserNotificationsHubService
    {
    }
}

