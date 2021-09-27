using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
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
            // TODO: Make this a method as its used in another place as well
            var cellGroupType = await _groupTypeRepo.Queryable().FirstOrDefaultAsync(x => x.Name == "Cell", ct);

            var cellGroups = await _dbRepository.Queryable()
                .AsNoTracking()
                .Where(x => x.GroupTypeId == cellGroupType.Id)
                .ToListAsync(ct);

            var totalCellsCount = cellGroups.Count;
            var activeCellsCount = cellGroups.Count(x => x.RecordStatus == RecordStatus.Active);
            var inActiveCellsCount = totalCellsCount - activeCellsCount;
            var onlineCellsCount = cellGroups.Count(x => x.IsOnline.HasValue && x.IsOnline.Value);

            var (peopleCount, leadersCount) = await _groupMemberDbRepository.PeopleAndLeadersInGroupsAsync(cellGroupType.Id);


            var date6Months = DateTime.UtcNow.AddMonths(-6);
            var openedCells = cellGroups
                .Count(x => x.StartDate >= date6Months);

            var closedCells = cellGroups
                .Count(
                    x => x.InactiveDateTime != null &&
                    x.InactiveDateTime >= date6Months &&
                    x.RecordStatus != RecordStatus.Active);

            return new ApiResponse(new { totalCellsCount, activeCellsCount, inActiveCellsCount, onlineCellsCount, peopleCount, leadersCount, openedCells, closedCells });
        }
    }
}
