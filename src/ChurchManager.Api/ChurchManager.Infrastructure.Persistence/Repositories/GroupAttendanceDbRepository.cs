using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Persistence.Models.Groups;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupAttendanceDbRepository : GenericRepositoryAsync<GroupAttendance>, IGroupAttendanceDbRepository
    {
        protected GroupAttendanceDbRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task RecordAttendanceAsync(DateTime meetingDate, IEnumerable<GroupMemberAttendance> groupMembers, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
