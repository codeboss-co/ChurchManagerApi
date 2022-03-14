using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features.Groups.Queries.Charts.Dashboard
{
    public class CellMinistryDashboardMetrics : IRequest<ApiResponse>
    {
    }

    public class CellMinistryDashboardMetricsHandler : IRequestHandler<CellMinistryDashboardMetrics, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IGroupMemberDbRepository _groupMemberDbRepository;
        private readonly IGroupMemberAttendanceDbRepository _groupMemberAttendanceDbRepository;
        private readonly IGenericDbRepository<GroupType> _groupTypeRepo;

        public CellMinistryDashboardMetricsHandler(
            IGroupDbRepository dbRepository,
            IGroupMemberDbRepository groupMemberDbRepository,
            IGroupMemberAttendanceDbRepository groupMemberAttendanceDbRepository,
            IGenericDbRepository<GroupType> groupTypeRepo)
        {
            _dbRepository = dbRepository;
            _groupMemberDbRepository = groupMemberDbRepository;
            _groupMemberAttendanceDbRepository = groupMemberAttendanceDbRepository;
            _groupTypeRepo = groupTypeRepo;
        }

        public async Task<ApiResponse> Handle(CellMinistryDashboardMetrics query, CancellationToken ct)
        {
            // TODO: Make this a method as its used in another place as well
            var cellGroupType = await _groupTypeRepo.Queryable().FirstOrDefaultAsync(x => x.Name == "Cell", ct);

            // TODO: fix doing this sql here and in the 'GroupStatisticsAsync' method again
            var cellGroups = await _dbRepository.Queryable()
                .AsNoTracking()
                .Where(x => x.GroupTypeId == cellGroupType.Id)
                .Select(x => new {x.Id})
                .ToListAsync(ct);

            // TODO: create class/structs for these
            var (totalCellsCount, activeCellsCount, inActiveCellsCount, onlineCellsCount, openedCells, closedCells) = await _dbRepository.GroupStatisticsAsync(cellGroupType.Id, DateTime.UtcNow.AddMonths(-6), ct);

            var (peopleCount, leadersCount) = await _groupMemberDbRepository.PeopleAndLeadersInGroupsAsync(cellGroupType.Id);

            var (newConvertsCount, firstTimersCount, holySpiritCount) = await _groupMemberAttendanceDbRepository.PeopleStatisticsAsync(cellGroups.Select(x => x.Id));

            return new ApiResponse(new
            {
                totalCellsCount, activeCellsCount, inActiveCellsCount, onlineCellsCount, peopleCount, leadersCount, openedCells, closedCells,
                newConvertsCount,
                firstTimersCount,
                holySpiritCount
            });
        }
    }
}
