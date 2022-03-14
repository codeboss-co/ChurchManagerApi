using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Groups.Events;
using ChurchManager.Domain.Features.Groups.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Features.Groups.Events.GroupAttendanceReviewed
{
    public class GroupAttendanceReviewedConsumer : IConsumer<GroupAttendanceReviewedEvent>
    {
        private readonly IGroupMemberDbRepository _dbRepository;
        private readonly IPushNotificationService _push;
        public ILogger<GroupAttendanceReviewedConsumer> Logger { get; }

        public GroupAttendanceReviewedConsumer(
            IGroupMemberDbRepository dbRepository,
            IPushNotificationService push,
            ILogger<GroupAttendanceReviewedConsumer> logger)
        {
            _dbRepository = dbRepository;
            _push = push;
            Logger = logger;
        }

        public async Task Consume(ConsumeContext<GroupAttendanceReviewedEvent> context)
        {
            Logger.LogInformation("------ GroupAttendanceReviewed event received ------");

            var message = context.Message;

            var leaders = await _dbRepository.GetLeaders(message.GroupId).ToListAsync();

            var notification = new PushNotification("Report Feedback" , message.Feedback);

            foreach (var leader in leaders)
            {
                await _push.SendNotificationToPersonAsync(leader.PersonId, notification);
            }
        }
    }
}
