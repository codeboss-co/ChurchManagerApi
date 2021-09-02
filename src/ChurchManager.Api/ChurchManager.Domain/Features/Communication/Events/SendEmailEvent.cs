using System.Collections.Generic;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communication.Events
{
    public record SendEmailEvent(string Subject, string Template, Person Recipient) : IDomainEvent
    {
        public IDictionary<string, string> TemplateData { get; set; }
    }
}
