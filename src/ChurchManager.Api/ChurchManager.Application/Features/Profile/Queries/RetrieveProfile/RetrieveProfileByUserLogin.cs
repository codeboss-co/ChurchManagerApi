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
    /// ProfileByUserloginIdQuery - handles media IRequest
    /// BaseRequestParameter - contains paging parameters
    /// To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public record ProfileByUserloginIdQuery(string UserLoginId) : IRequest<ApiResponse>
    {
    }

    public class RetrieveProfileByUserLogin : IRequestHandler<ProfileByUserloginIdQuery, ApiResponse>
    {
        private readonly IPersonDbRepository _personDbRepository;
        private readonly IMapper _mapper;

        public RetrieveProfileByUserLogin(IPersonDbRepository personDbRepository, IMapper mapper)
        {
            _personDbRepository = personDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ProfileByUserloginIdQuery query, CancellationToken cancellationToken)
        {
            var domain = await _personDbRepository.ProfileByUserLoginId(query.UserLoginId);
            
            return domain is null 
                ? new ApiResponse("No matching user login Id found")
                : new ApiResponse(_mapper.Map<PersonViewModel>(domain));
        }
    }
}
