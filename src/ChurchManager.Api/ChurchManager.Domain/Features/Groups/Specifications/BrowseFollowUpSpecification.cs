using System;
using System.Linq;
using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class BrowseFollowUpSpecification : Specification<FollowUp, FollowUpViewModel>
    {
        public BrowseFollowUpSpecification(
            IPagedQuery paging,
            int? personId,
            int? assignedPersonId,
            string type,
            string[] severity,
            bool? withAction,
            DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include(x => x.Person);

            // Person Filter
            if(personId.HasValue)
            {
                Query.Where(g => g.PersonId == personId);
            }

            // Assigned Person Filter
            if(assignedPersonId.HasValue)
            {
                Query.Where(g => g.AssignedPersonId == assignedPersonId);
            }

            // Type Filter
            if (!type.IsNullOrEmpty())
            {
                Query.Where(g => g.Type == type);
            }

            // Severity Filter
            if(severity.Any())
            {
                Query.Where(g => severity.Contains(g.Severity));
            }

            // Include those with Actions
            if (withAction.HasValue && withAction.Value)
            {
                Query.Where(g => g.ActionDate != null);
            }
            else
            {
                Query.Where(g => g.ActionDate == null);
            }

            // Date Filters
            if(from.HasValue)
            {
                Query.Where(g => g.AssignedDate >= from.Value);
            }
            if(to.HasValue)
            {
                Query.Where(g => g.AssignedDate <= to.Value);
            }

            Query.OrderBy(x => x.AssignedDate);

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());

            Query.Select(x => new FollowUpViewModel
            {
                Id = x.Id,
                AssignedDate = x.AssignedDate,
                ActionDate = x.ActionDate,
                Type = x.Type,
                Person = new PeopleAutocompleteViewModel(x.PersonId, x.Person.FullName.ToString(), x.Person.PhotoUrl, x.Person.ConnectionStatus),
                AssignedPerson = new PeopleAutocompleteViewModel(x.AssignedPersonId, x.AssignedPerson.FullName.ToString(), x.AssignedPerson.PhotoUrl, x.AssignedPerson.ConnectionStatus),
                Severity = x.Severity,
                Note = x.Note,
                RequiresAdditionalFollowUp = x.RequiresAdditionalFollowUp
            });
        }  
    }
}
