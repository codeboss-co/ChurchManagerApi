﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;

namespace ChurchManager.Application.Features.FollowUp.Queries.BrowseFollowUp
{
    public record BrowseFollowUpQuery
        : SearchTermQueryParameter, IRequest<PagedResponse<FollowUpViewModel>>
    {
        public string Type { get; set; }
        public AutocompleteResult Person { get; set; }
        public AutocompleteResult AssignedPerson { get; set; }
        public string[] Severity { get; set; } =  {};
        public bool? WithAction { get; set; }
        public bool? AssignedToMe { get; set; } // Assigned to current user
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class BrowseFollowUp : IRequestHandler<BrowseFollowUpQuery, PagedResponse<FollowUpViewModel>>
    {
        private readonly IGenericDbRepository<Domain.Features.People.FollowUp> _dbRepository;
        private readonly ICognitoCurrentUser _currentUser;

        public BrowseFollowUp(
            IGenericDbRepository<Domain.Features.People.FollowUp> dbRepository,
            ICognitoCurrentUser currentUser)
        {
            _dbRepository = dbRepository;
            _currentUser = currentUser;
        }

        public async Task<PagedResponse<FollowUpViewModel>> Handle(BrowseFollowUpQuery query, CancellationToken ct)
        {
            int? assignedPersonId = query.AssignedPerson?.Id;
            if (query.AssignedToMe.HasValue && query.AssignedToMe.Value)
            {
                assignedPersonId = _currentUser.PersonId;
            }

            var spec = new BrowseFollowUpSpecification(
                query, 
                query.Person?.Id,
                assignedPersonId, 
                query.Type, 
                query.Severity,
                query.WithAction, 
                query.From, query.To);

            var results = await _dbRepository.BrowseAsync<FollowUpViewModel>(query, spec, ct);

            return new PagedResponse<FollowUpViewModel>(results);

        }
    }
}
