using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using MediatR;

namespace ChurchManager.Application.Features.Profile.Queries.RetrieveProfile
{
    /// <summary>
    /// ProfileByUserLoginIdQuery - handles media IRequest
    /// BaseRequestParameter - contains paging parameters
    /// To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public record ProfileByUserLoginIdQuery(string UserLoginId) : IRequest<ApiResponse>
    {
    }

    public class ProfileByUserLogin : IRequestHandler<ProfileByUserLoginIdQuery, ApiResponse>
    {
        private readonly IPersonDbRepository _personDbRepository;
        private readonly IDiscipleshipStepDefinitionDbRepository _stepsDbRepository;
        private readonly IMapper _mapper;

        public ProfileByUserLogin(
            IPersonDbRepository personDbRepository,
            IDiscipleshipStepDefinitionDbRepository stepsDbRepository,
            IMapper mapper)
        {
            _personDbRepository = personDbRepository;
            _stepsDbRepository = stepsDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ProfileByUserLoginIdQuery query, CancellationToken ct)
        {
            var domain = await _personDbRepository.ProfileByUserLoginId(query.UserLoginId);

            // Foundation School status
            if(domain is not null)
            {
                var foundationSchoolStep = await _stepsDbRepository.DiscipleshipStepInfoForPersonAsync(domain.PersonId, 1, ct);

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
