using ChurchManager.Infrastructure.Abstractions.MassTransit;
using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public abstract class MassTransitSignalRHubService<THub> :
        IHubTransportService<IPublishEndpoint>,
        IUserHubService
        where THub : Hub
    {
        private readonly IReadOnlyList<IHubProtocol> _protocols = new IHubProtocol[] { new JsonHubProtocol() };

        public async Task SendToUserAsync<TModel>(
            TModel model,
            string userId,
            string methodName,
            IPublishEndpoint publisher)
        {
            await publisher.Publish<User<THub>>(new
            {
                UserId = userId,
                Messages = this._protocols.ToProtocolDictionary(methodName, new object[]
                {
                    model
                })
            }).ConfigureAwait(false);
        }
    }
}
