using System;
using System.Linq;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Churches.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Churches;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class ChurchAttendanceDbRepository : GenericRepositoryAsync<ChurchAttendance>, IChurchAttendanceDbRepository
    {
        public ChurchAttendanceDbRepository(IChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<dynamic> DashboardChurchAttendanceAsync(DateTime from, DateTime to)
        {
            return await Queryable()
                .Where(x => x.AttendanceDate >= from && x.AttendanceDate <= to)
                .Select(x => new
                {
                    x.AttendanceDate,
                    x.AttendanceCount
                })
                .GroupBy(x => new
                    {
                        Year = x.AttendanceDate.Year,
                        Month = x.AttendanceDate.Month
                    },
                    (x, e) => new
                    {
                        x.Year,
                        x.Month,
                        TotalAttendance = e.Sum(y => y.AttendanceCount)
                    })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync();
        }
    }
}
