using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.People.Queries.BrowsePeople
{
    public record BrowsePeopleQuery : SearchTermQueryParameter, IRequest<PagedResponse<object>>
    {
    }

    public class BrowsePeopleHandler : IRequestHandler<BrowsePeopleQuery, PagedResponse<object>>
    {
        private readonly IPersonDbRepository _dbRepository;

        public BrowsePeopleHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PagedResponse<object>> Handle(BrowsePeopleQuery query, CancellationToken ct)
        {
            var pagedResult = await _dbRepository.BrowsePeopleAsync(query, ct);

            return new PagedResponse<object>(pagedResult);
        }
    }
}
