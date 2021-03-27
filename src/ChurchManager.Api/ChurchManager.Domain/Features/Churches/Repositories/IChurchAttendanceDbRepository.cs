using System;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Churches;

namespace ChurchManager.Domain.Features.Churches.Repositories
{
    public interface IChurchAttendanceDbRepository : IGenericRepositoryAsync<ChurchAttendance>
    {
        Task<dynamic> DashboardChurchAttendanceAsync(DateTime from, DateTime to);
    }
}
