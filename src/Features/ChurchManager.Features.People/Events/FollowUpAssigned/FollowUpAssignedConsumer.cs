using ChurchManager.Domain.Common.Configuration;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.People.Events;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Types;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChurchManager.Features.People.Events.FollowUpAssigned
{
    public class FollowUpAssignedConsumer : IConsumer<FollowUpAssignedEvent>
    {
        private readonly IGenericDbRepository<Domain.Features.People.FollowUp> _dbRepository;
        private readonly IPersonDbRepository _personDbRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly BugsnagOptions _bugsnagOptions;
        public ILogger<FollowUpAssignedConsumer> Logger { get; }

        public FollowUpAssignedConsumer(
            IGenericDbRepository<Domain.Features.People.FollowUp> dbRepository,
            IPersonDbRepository personDbRepository,
            IDateTimeProvider dateTimeProvider,
            IOptions<BugsnagOptions> bugsnagOptions,
            ILogger<FollowUpAssignedConsumer> logger)
        {
            Logger = logger;
            _dbRepository = dbRepository;
            _personDbRepository = personDbRepository;
            _dateTimeProvider = dateTimeProvider;
            _bugsnagOptions = bugsnagOptions.Value;
        }

        public async Task Consume(ConsumeContext<FollowUpAssignedEvent> context)
        {
            var message = context.Message;

            Logger.LogInformation("------ FollowUpAssignedEvent event received {@message)------", message);

            await _dbRepository.AddAsync(new Domain.Features.People.FollowUp
            {
                PersonId = message.PersonId,
                AssignedPersonId = message.AssignedFollowUpPersonId,
                Type = message.Type,
                // Audits
                CreatedBy = message.UserLoginId,
                CreatedDate = DateTime.UtcNow
            });

            if(message.SendEmail)
            {
                var followUpAssignedPerson = await _personDbRepository.GetByIdAsync(message.AssignedFollowUpPersonId);

                var templateData = new Dictionary<string, string>
                {
                    ["Title"] = followUpAssignedPerson!.FullName.Title,
                    ["FirstName"] = followUpAssignedPerson!.FullName.FirstName,
                    ["LastName"] = followUpAssignedPerson!.FullName.LastName,
                    ["PersonUrl"] = GeneratedPersonUrl(message.PersonId),
                    ["CreationDate"] = _dateTimeProvider.ConvertFromUtc(DateTime.UtcNow).ToShortTimeString()
                };

                await context.Publish(new SendEmailEvent(
                    "Follow Up Assignment",
                    DomainConstants.Communication.Email.Templates.FollowUpTemplate,
                    followUpAssignedPerson)
                {
                    TemplateData = templateData
                });
            }

        }

        string GeneratedPersonUrl(int personId) => _bugsnagOptions switch
            {
                { ReleaseStage: "Development" } => $"http://localhost:4200/pages/profile/{personId}",
                { ReleaseStage: "Test" } => $"https://test-churchmanager.codeboss.tech/pages/profile/{personId}",
                { ReleaseStage: "Production" } => $"https://churchmanager.codeboss.tech//pages/profile/{personId}",
                _ => $"http://localhost:4200/person/{personId}"
            };

    }
}



