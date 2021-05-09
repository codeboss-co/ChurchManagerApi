using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared.Parameters;
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

        public async Task<ApiResponse> Handle(PeopleAutocompleteQuery request, CancellationToken ct)
        {
            var results = await _dbRepository.AutocompleteAsync(request.SearchTerm, ct);

            return new ApiResponse(results);
        }
    }
}
