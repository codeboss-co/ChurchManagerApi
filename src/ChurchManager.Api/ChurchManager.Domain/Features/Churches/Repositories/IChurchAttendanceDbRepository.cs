using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Churches;

namespace ChurchManager.Domain.Features.Churches.Repositories
{
    public interface IChurchAttendanceDbRepository : IGenericRepositoryAsync<ChurchAttendance>
    {
        Task<IEnumerable<ChurchAttendanceAnnualBreakdownVm>> DashboardChurchAttendanceAsync(DateTime from, DateTime to);
        Task<dynamic> DashboardChurchAttendanceBreakdownAsync(DateTime from, DateTime to);
    }
}
