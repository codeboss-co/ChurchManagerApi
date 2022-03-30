using ChurchManager.Domain.Features.Missions;
using ChurchManager.Domain.Features.Missions.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;


namespace ChurchManager.Features.Missions.Queries.BrowseMissions
{
    public record BrowseMissionsQuery : QueryParameter, IRequest<PagedResponse<MissionViewModel>>
    {
        public int? PersonId { get; set; }
        public int? GroupId { get; set; }
        public int? ChurchId { get; set; }
        public string[] Types { get; set; }
        public string[] Categories { get; set; }
        public string[] Streams { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    
    }

    public class BrowseMissionsHandler : IRequestHandler<BrowseMissionsQuery, PagedResponse<MissionViewModel>>
    {
        private readonly IGenericDbRepository<Mission> _dbRepository;

        public BrowseMissionsHandler(IGenericDbRepository<Mission> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PagedResponse<MissionViewModel>> Handle(BrowseMissionsQuery query, CancellationToken ct)
        {
            var spec = new BrowseMissionsSpecification(query, query.PersonId, query.GroupId, query.ChurchId, query.Types, query.Categories, query.Streams, query.From, query.To);

            var pagedResult = await _dbRepository.BrowseAsync<MissionViewModel>(query, spec, ct);

            return new PagedResponse<MissionViewModel>(pagedResult);
        }
    }
}
