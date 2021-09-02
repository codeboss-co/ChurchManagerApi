using System.Threading.Tasks;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Events;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Features.People.Events.FollowUpAssigned
{
    public class FollowUpAssignedConsumer : IConsumer<FollowUpAssignedEvent>
    {
        private readonly IGenericDbRepository<FollowUp> _dbRepository;
        private readonly ChurchManagerDbContext _dbContext;
        public ILogger<FollowUpAssignedConsumer> Logger { get; }

        public FollowUpAssignedConsumer(
            IGenericDbRepository<FollowUp> dbRepository,
            ChurchManagerDbContext dbContext,
            ILogger<FollowUpAssignedConsumer> logger)
        {
            Logger = logger;
            _dbRepository = dbRepository;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<FollowUpAssignedEvent> context)
        {
            var message = context.Message;

            Logger.LogInformation("------ FollowUpAssignedEvent event received {@message)------", message);

            await _dbRepository.AddAsync(new FollowUp
            {
                PersonId = message.PersonId,
                AssignedPersonId = message.AssignedFollowUpPersonId,
                Type = message.Type
            });
        }
    }
}
