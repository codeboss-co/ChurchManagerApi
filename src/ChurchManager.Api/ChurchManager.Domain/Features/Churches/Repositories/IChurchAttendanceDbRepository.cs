using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Churches.Repositories
{
    public interface IChurchAttendanceDbRepository : IGenericDbRepository<ChurchAttendance>
    {
        Task<IEnumerable<ChurchAttendanceAnnualBreakdownVm>> DashboardChurchAttendanceAsync(DateTime from, DateTime to);
        Task<dynamic> DashboardChurchAttendanceBreakdownAsync(DateTime from, DateTime to);
    }
}
