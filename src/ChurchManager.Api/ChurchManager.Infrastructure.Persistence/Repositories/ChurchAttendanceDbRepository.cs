using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
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

        public async Task<IEnumerable<ChurchAttendanceAnnualBreakdownVm>> DashboardChurchAttendanceAsync(DateTime from, DateTime to)
        {
            var raw = await Queryable()
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
                    (x, e) => new ChurchAttendanceMonthlyTotalsVm
                    {
                        Year = x.Year,
                        Month = x.Month,
                        TotalAttendance = e.Sum(y => y.AttendanceCount)
                    })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync();

            return raw
                .GroupBy(x => x.Year)
                .Select( x => new ChurchAttendanceAnnualBreakdownVm
                {
                    Year = x.Key,
                    Data = x
                });
        }
    }
}
