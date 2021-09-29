using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupMemberAttendanceDbRepository : GenericRepositoryBase<GroupMemberAttendance>, IGroupMemberAttendanceDbRepository
    {
        public GroupMemberAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(int newConvertsCount, int firstTimersCount, int holySpiritCount)> PeopleStatisticsAsync(IEnumerable<int> groupIds, DateTime startDate = default)
        {
            if (startDate == default)
            {
                startDate = DateTime.UtcNow.AddMonths(-6);
            }

            var query = Queryable()
                .Where(x =>
                    groupIds.Contains(x.GroupId) &&
                    x.AttendanceDate >= startDate);

            var newConvertsCount = await query.CountAsync(x => x.IsNewConvert.HasValue && x.IsNewConvert.Value);
            var firstTimersCount = await query.CountAsync(x => x.IsFirstTime.HasValue && x.IsFirstTime.Value);
            var holySpiritCount = await query.CountAsync(x => x.ReceivedHolySpirit.HasValue && x.ReceivedHolySpirit.Value);

            return (newConvertsCount, firstTimersCount, holySpiritCount);
        }
    }
}
