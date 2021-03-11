using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared;
using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.Profile.Queries.RetrieveProfile
{
    /// <summary>
    /// ProfileByUserLoginIdQuery - handles media IRequest
    /// BaseRequestParameter - contains paging parameters
    /// To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public record UserDetailsByUserLoginQuery(string UserLoginId) : IRequest<ApiResponse>
    {
    }

    public class UserDetailsByUserLogin : IRequestHandler<UserDetailsByUserLoginQuery, ApiResponse>
    {
        private readonly IPersonDbRepository _personDbRepository;
        private readonly IMapper _mapper;

        public UserDetailsByUserLogin(IPersonDbRepository personDbRepository, IMapper mapper)
        {
            _personDbRepository = personDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(UserDetailsByUserLoginQuery query, CancellationToken cancellationToken)
        {
            UserDetails user = await _personDbRepository.UserDetailsByUserLoginId(query.UserLoginId);
            
            return user is null 
                ? new ApiResponse("No matching user found")
                : new ApiResponse(user);
        }
    }
}
