using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using MediatR;

namespace ChurchManager.Application.Features.People.Queries.FindDuplicates
{
    public record FindPeopleDuplicatesQuery : IRequest<ApiResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class PeopleDuplicateHandler : IRequestHandler<FindPeopleDuplicatesQuery, ApiResponse>
    {
        private readonly IPersonDbRepository _dbRepository;

        public PeopleDuplicateHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(FindPeopleDuplicatesQuery query, CancellationToken ct)
        {
            var searchParams = new PersonMatchQuery(query.FirstName, query.LastName, query.Email, null);
            
            var spec = new FindPeopleSpecification(searchParams);

            var people = await _dbRepository.ListAsync(spec, ct);

            return new ApiResponse(people?.Any() ?? false);
        }
    }
}
