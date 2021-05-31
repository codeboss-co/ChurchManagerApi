﻿using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Infrastructure.Abstractions
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(IDomainEvent @event, CancellationToken ct = default);
    }
}
