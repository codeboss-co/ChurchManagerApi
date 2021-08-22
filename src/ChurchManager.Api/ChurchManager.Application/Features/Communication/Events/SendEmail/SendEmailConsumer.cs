using System;
using System.IO;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Communication.Events;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Communication;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Application.Features.Communication.Events.SendEmail
{
    public class SendEmailConsumer : IConsumer<SendEmailEvent>
    {
        private readonly IEmailSender _sender;
        private readonly ITemplateParser _templateParser;
        public ILogger<SendEmailConsumer> Logger { get; }


        public SendEmailConsumer(
            IEmailSender sender,
            ITemplateParser templateParser,
            ILogger<SendEmailConsumer> logger)
        {
            _sender = sender;
            _templateParser = templateParser;
            Logger = logger;
        }

        public async Task Consume(ConsumeContext<SendEmailEvent> context)
        {
            Logger.LogInformation("------ SendEmailConsumer event received ------");
            
            var message = context.Message;

            if(_isEmailActive(message.Recepient))
            {
                string template = await File.ReadAllTextAsync(DomainConstants.Communication.Email.Template(message.Template));
                object model = new { Model = message.TemplateData };

                var htmlBody = _templateParser.Render(template, model);

                var operationResult = await _sender.SendEmailAsync(message.Recepient.Email.Address, message.Subject, htmlBody);

                if (!operationResult.IsSuccess)
                {
                   // Raise event to set their email address to inactive to avoid repeated bounces
                }
            }
        }

        private readonly Predicate<Person> _isEmailActive = person => person.Email?.IsActive != null && person.Email.IsActive.Value;
    }
}
