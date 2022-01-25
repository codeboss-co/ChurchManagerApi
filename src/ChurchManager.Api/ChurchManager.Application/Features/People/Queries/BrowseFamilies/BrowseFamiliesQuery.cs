using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features.People.Queries.BrowseFamilies
{
    public record BrowseFamiliesQuery : QueryParameter, IRequest<PagedResponse<FamilyViewModel>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class BrowseFamiliesHandler : IRequestHandler<BrowseFamiliesQuery, PagedResponse<FamilyViewModel>>
    {
        private readonly IGenericDbRepository<Family> _dbRepository;

        public BrowseFamiliesHandler(IGenericDbRepository<Family> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PagedResponse<FamilyViewModel>> Handle(BrowseFamiliesQuery query, CancellationToken ct)
        {
            var spec = new BrowseFamiliesSpecification(query, query.Name, query.Address);

            var pagedResult = await _dbRepository.BrowseAsync<FamilyViewModel>(query, spec, ct);

            return new PagedResponse<FamilyViewModel>(pagedResult);
        }
    }
}
