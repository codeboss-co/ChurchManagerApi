using System.Threading.Tasks;
using ChurchManager.Domain.Features.Groups.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Features.Groups.Events.GroupAttendanceReviewed
{
    public class GroupAttendanceReviewedConsumer : IConsumer<GroupAttendanceReviewedEvent>
    {
        public ILogger<GroupAttendanceReviewedConsumer> Logger { get; }

        public GroupAttendanceReviewedConsumer(ILogger<GroupAttendanceReviewedConsumer> logger)
        {
            Logger = logger;
        }

        public Task Consume(ConsumeContext<GroupAttendanceReviewedEvent> context)
        {
            Logger.LogInformation("------ GroupAttendanceReviewed event received ------");

            return Task.CompletedTask;
        }
    }
}
