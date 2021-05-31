using System.Threading.Tasks;
using ChurchManager.Domain.Shared;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Tests
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
            Logger.LogInformation(context.Message.Content);
            return Task.CompletedTask;
        }
    }

    public class TestDomainEvent : IDomainEvent
    {
        public string Content { get; set; } = "Hello";
    }
}
