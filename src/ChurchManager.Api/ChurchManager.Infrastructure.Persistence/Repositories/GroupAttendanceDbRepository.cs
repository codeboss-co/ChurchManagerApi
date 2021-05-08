using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Domain.Specifications;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupAttendanceDbRepository : GenericRepositoryAsync<GroupAttendance>, IGroupAttendanceDbRepository
    {
        public GroupAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<GroupAttendanceViewModel>> BrowseGroupAttendance(
            QueryParameter query,
            int groupTypeId,
            int? churchId,
            int? groupId,
            bool withFeedback,
            DateTime? from, DateTime? to,
            CancellationToken ct = default)
        {
            // Paging
            var pagedResult = await Queryable()
                .Specify(new BrowseGroupAttendanceSpecification(groupTypeId, churchId, groupId, withFeedback, from, to))
                .PaginateAsync(query);

            return PagedResult<GroupAttendanceViewModel>.Create(
                pagedResult.Items.Select(x => new GroupAttendanceViewModel
                {
                    Id = x.Id,
                    GroupName = x.Group.Name,
                    AttendanceDate = x.AttendanceDate,
                    DidNotOccur = x.DidNotOccur,
                    AttendanceCount = x.AttendanceCount,
                    FirstTimerCount = x.FirstTimerCount,
                    NewConvertCount = x.NewConvertCount,
                    ReceivedHolySpiritCount = x.ReceivedHolySpiritCount,
                    Notes = x.Notes,
                    PhotoUrls = x.PhotoUrls
                }),
                pagedResult.CurrentPage,
                pagedResult.ResultsPerPage,
                pagedResult.TotalPages,
                pagedResult.TotalResults);
        }

        public  async Task<dynamic> WeeklyBreakdownForPeriodAsync(int? groupId, ReportPeriod reportPeriod, CancellationToken ct)
        {
            var queryable = Queryable()
                .Where(x => x.DidNotOccur == null || x.DidNotOccur.Value != true);

            switch (reportPeriod)
            {
                case ReportPeriod.Month:
                    var monthAgo = DateTime.Now.AddMonths(-1);
                    var now = DateTime.Now;
                    queryable = queryable.Where(x => 
                        x.AttendanceDate >= monthAgo &&
                        x.AttendanceDate <= now);
                    break;

                default:
                    var sixMonthsAgo = DateTime.Now.AddMonths(-6);
                    queryable = queryable.Where(x =>
                        x.AttendanceDate >= sixMonthsAgo);
                    break;
            }

            if (groupId.HasValue)
            {
                queryable = queryable.Where(x => x.GroupId == groupId.Value);
            }

            // https://entityframeworkcore.com/knowledge-base/53307101/group-by-week-ef-core-2-1
            return await queryable
                .Select(x => new
                {
                    x.AttendanceDate,
                    x.AttendanceCount,
                    x.NewConvertCount,
                    x.FirstTimerCount,
                    x.ReceivedHolySpiritCount
                })
                .GroupBy(x => new
                    {
                        Week = 1 + (x.AttendanceDate.DayOfYear - 1) / 7
                    },
                    (x, e) => new
                    {
                        x.Week,
                        TotalAttendance = e.Sum(y => y.AttendanceCount),
                        TotalNewConverts = e.Sum(y => y.NewConvertCount),
                        TotalFirstTimers = e.Sum(y => y.FirstTimerCount),
                        TotalHolySpirit = e.Sum(y => y.ReceivedHolySpiritCount),
                    })
                .OrderBy(x => x.Week)
                .ToListAsync(ct);
        }
    }
}
