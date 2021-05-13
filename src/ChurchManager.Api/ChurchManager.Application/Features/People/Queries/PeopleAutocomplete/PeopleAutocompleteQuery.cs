using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Parameters;
using MediatR;

namespace ChurchManager.Application.Features.People.Queries.PeopleAutocomplete
{
    public record PeopleAutocompleteQuery : SearchTermQueryParameter, IRequest<ApiResponse>;

    public class PeopleAutocompleteResults : IRequestHandler<PeopleAutocompleteQuery, ApiResponse>
    {
        private readonly IPersonDbRepository _dbRepository;

        public PeopleAutocompleteResults(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(PeopleAutocompleteQuery query, CancellationToken ct)
        {
            var spec = new PeopleAutocompleteSpecification(query.SearchTerm);
            var results = await _dbRepository.ListAsync(spec, ct);

            return new ApiResponse(results);
        }
    }
}
