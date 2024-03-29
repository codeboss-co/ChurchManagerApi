﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupAttendanceDbRepository : IGenericDbRepository<GroupAttendance>
    {
        Task<PagedResult<GroupAttendanceViewModel>> BrowseGroupAttendance(
            QueryParameter query, 
            int groupTypeId,
            int? churchId,
            int? groupId,
            bool withFeedback,
            DateTime? from, DateTime? to,
            CancellationToken ct = default);

        Task<dynamic> WeeklyBreakdownForPeriodAsync(int? groupId, ReportPeriod reportPeriod, CancellationToken ct);
    }
}
