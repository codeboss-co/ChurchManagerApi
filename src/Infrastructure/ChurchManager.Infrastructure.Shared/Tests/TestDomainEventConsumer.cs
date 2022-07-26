using ChurchManager.Domain.Shared;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Tests
{
    public class TestDomainEventConsumer : IConsumer<TestDomainEvent>
    {
        public ILogger<TestDomainEventConsumer> Logger { get; }

        public TestDomainEventConsumer(ILogger<TestDomainEventConsumer> logger)
        {
            Logger = logger;
        }

        public Task Consume(ConsumeContext<TestDomainEvent> context)
        {
            Logger.LogInformation($"✔️ Message Received: {context.Message.Content}");
            return Task.CompletedTask;
        }
    }

    public class TestDomainEvent : IDomainEvent
    {
        public string Content { get; set; } = "Hello";
    }
}
