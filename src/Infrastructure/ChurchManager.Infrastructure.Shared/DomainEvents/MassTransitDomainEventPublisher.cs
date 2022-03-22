using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions;
using MassTransit;

namespace ChurchManager.Infrastructure.Shared.DomainEvents
{
    public class MassTransitDomainEventPublisher : IDomainEventPublisher
    {
        private readonly IPublishEndpoint _endpoint;

        public MassTransitDomainEventPublisher(IPublishEndpoint endpoint) => _endpoint = endpoint;

        public Task PublishAsync(IDomainEvent @event, CancellationToken ct = default)
        {
            var type = @event.GetType();
            return _endpoint.Publish(@event, type, ct);
        }
    }
}
