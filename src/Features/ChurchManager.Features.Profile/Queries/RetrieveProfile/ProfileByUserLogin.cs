using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Profile.Queries.RetrieveProfile
{
    /// <summary>
    ///     ProfileByUserLoginIdQuery - handles media IRequest
    ///     BaseRequestParameter - contains paging parameters
    ///     To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public record ProfileByUserLoginIdQuery(string UserLoginId) : IRequest<ApiResponse>
    {
    }

    public class ProfileByUserLogin : IRequestHandler<ProfileByUserLoginIdQuery, ApiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProfileService _service;
        private readonly IDiscipleshipStepDefinitionDbRepository _stepsDbRepository;

        public ProfileByUserLogin(
            IProfileService service,
            IDiscipleshipStepDefinitionDbRepository stepsDbRepository,
            IMapper mapper)
        {
            _service = service;
            _stepsDbRepository = stepsDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ProfileByUserLoginIdQuery query, CancellationToken ct)
        {
            var domain = await _service.ProfileByUserLoginId(query.UserLoginId, ct);

            // Foundation School status
            if (domain is not null)
            {
                var foundationSchoolStep =
                    await _stepsDbRepository.DiscipleshipStepInfoForPersonAsync(domain.PersonId, 1, ct);

                domain.FoundationSchool = foundationSchoolStep.FirstOrDefault() ?? new DiscipleshipStepViewModel
                {
                    IsComplete = false,
                    Status = "Not Started"
                };
            }

            return domain is null
                ? new ApiResponse("No matching user login Id found")
                : new ApiResponse(_mapper.Map<PersonViewModel>(domain));
        }
    }
}