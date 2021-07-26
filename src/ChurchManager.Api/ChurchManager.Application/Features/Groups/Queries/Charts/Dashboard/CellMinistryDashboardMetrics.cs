using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Queries.Charts.Dashboard
{
    public class CellMinistryDashboardMetrics : IRequest<ApiResponse>
    {
    }

    public class CellMinistryDashboardMetricsHandler : IRequestHandler<CellMinistryDashboardMetrics, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IGroupMemberDbRepository _groupMemberDbRepository;
        private readonly IGenericDbRepository<GroupType> _groupTypeRepo;

        public CellMinistryDashboardMetricsHandler(
            IGroupDbRepository dbRepository,
            IGroupMemberDbRepository groupMemberDbRepository,
            IGenericDbRepository<GroupType> groupTypeRepo)
        {
            _dbRepository = dbRepository;
            _groupMemberDbRepository = groupMemberDbRepository;
            _groupTypeRepo = groupTypeRepo;
        }

        public async Task<ApiResponse> Handle(CellMinistryDashboardMetrics query, CancellationToken ct)
        {
            var activeOrOnlineCells = await _dbRepository.Queryable()
                .AsNoTracking()
                .Where(x =>
                    x.IsOnline.HasValue && x.IsOnline.Value ||
                    x.RecordStatus == RecordStatus.Active)
                .ToListAsync(ct);

            var activeCellsCount = activeOrOnlineCells.Count(x => x.RecordStatus == RecordStatus.Active);
            var onlineCellsCount = activeOrOnlineCells.Count(x => x.IsOnline.HasValue && x.IsOnline.Value);

            // TODO: Make this a method as its used in another place as well
            var cellGroupType = await _groupTypeRepo.Queryable().FirstOrDefaultAsync(x => x.Name == "Cell", ct);
            var (peopleCount, leadersCount) = await _groupMemberDbRepository.PeopleAndLeadersInGroupsAsync(cellGroupType.Id);

            return new ApiResponse(new { activeCellsCount, onlineCellsCount, peopleCount, leadersCount });
        }
    }
}
