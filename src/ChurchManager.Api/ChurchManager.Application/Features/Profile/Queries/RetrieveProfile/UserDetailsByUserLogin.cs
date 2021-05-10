using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ChurchManager.Application.Common;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
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
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IMapper _mapper;

        public UserDetailsByUserLogin(
            IPersonDbRepository personDbRepository,
            ICognitoCurrentUser currentUser,
            IMapper mapper)
        {
            _personDbRepository = personDbRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(UserDetailsByUserLoginQuery query, CancellationToken ct)
        {
            var spec = new UserDetailsSpecification(query.UserLoginId);

            var user = await _personDbRepository.GetBySpecAsync<UserDetails>(spec, ct);

            if (user is null)
            {
                return new ApiResponse("No matching user found");
            }
            
            // Set missing properties from the logged in user
            user.Username = _currentUser.Username;
            return new ApiResponse(user);
        }
    }
}
