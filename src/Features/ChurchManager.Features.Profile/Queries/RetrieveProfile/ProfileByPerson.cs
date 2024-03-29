﻿using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Profile.Queries.RetrieveProfile
{
    /// <summary>
    ///     ProfileByPersonIdQuery - handles media IRequest
    ///     BaseRequestParameter - contains paging parameters
    ///     To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public record ProfileByPersonIdQuery(int PersonId, bool Condensed) : IRequest<ApiResponse>
    {
    }

    public class ProfileByPerson : IRequestHandler<ProfileByPersonIdQuery, ApiResponse>
    {
        private readonly IProfileService _service;
        private readonly IDiscipleshipStepDefinitionDbRepository _stepsDbRepository;

        public ProfileByPerson(
            IProfileService service,
            IDiscipleshipStepDefinitionDbRepository stepsDbRepository,
            IMapper mapper)
        {
            _service = service;
            _stepsDbRepository = stepsDbRepository;
        }

        public async Task<ApiResponse> Handle(ProfileByPersonIdQuery query, CancellationToken ct)
        {
            var vm = await _service.ProfileByPersonId(query.PersonId, query.Condensed, ct);

            // Foundation School status
            if (vm is not null)
            {
                // TODO: Get from database
                var foundationSchoolStepDefinitionId = 1;
                var foundationSchoolStep =
                    await _stepsDbRepository.DiscipleshipStepInfoForPersonAsync(vm.PersonId, foundationSchoolStepDefinitionId, ct);

                vm.FoundationSchool = foundationSchoolStep.FirstOrDefault() ?? new DiscipleshipStepViewModel
                {
                    IsComplete = false,
                    Status = "Not Started"
                };
            }

            return vm is null
                ? new ApiResponse("No matching user login Id found")
                : new ApiResponse(vm);
        }
    }
}