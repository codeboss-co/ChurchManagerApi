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
    /// ProfileByPersonIdQuery - handles media IRequest
    /// BaseRequestParameter - contains paging parameters
    /// To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public record ProfileByPersonIdQuery(int PersonId) : IRequest<ApiResponse>
    {
    }

    public class ProfileByPerson : IRequestHandler<ProfileByPersonIdQuery, ApiResponse>
    {
        private readonly IPersonDbRepository _personDbRepository;
        private readonly IMapper _mapper;

        public ProfileByPerson(IPersonDbRepository personDbRepository, IMapper mapper)
        {
            _personDbRepository = personDbRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(ProfileByPersonIdQuery query, CancellationToken cancellationToken)
        {
            var domain = await _personDbRepository.ProfileByPersonId(query.PersonId);
            
            return domain is null 
                ? new ApiResponse("No matching user login Id found")
                : new ApiResponse(_mapper.Map<PersonViewModel>(domain));
        }
    }
}
