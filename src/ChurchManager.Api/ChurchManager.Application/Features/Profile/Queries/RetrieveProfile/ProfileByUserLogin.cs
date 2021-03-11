using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Features.People.Queries;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People.Repositories;
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
        private readonly IMapper _mapper;

        public ProfileByUserLogin(IPersonDbRepository personDbRepository, IMapper mapper)
        {
            _personDbRepository = personDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ProfileByUserLoginIdQuery query, CancellationToken cancellationToken)
        {
            var domain = await _personDbRepository.ProfileByUserLoginId(query.UserLoginId);
            
            return domain is null 
                ? new ApiResponse("No matching user login Id found")
                : new ApiResponse(_mapper.Map<PersonViewModel>(domain));
        }
    }
}
