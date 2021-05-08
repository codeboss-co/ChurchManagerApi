using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupAttendanceDbRepository : IGenericRepositoryAsync<GroupAttendance>
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
