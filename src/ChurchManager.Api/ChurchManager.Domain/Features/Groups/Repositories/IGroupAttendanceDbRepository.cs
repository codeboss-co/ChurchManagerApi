using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Groups;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupAttendanceDbRepository : IGenericRepositoryAsync<GroupAttendance>
    {
        Task<PagedResult<object>> BrowseGroupAttendance(
            QueryParameter query, int groupTypeId, int? churchId, bool? withFeedback, DateTime? from, DateTime? to, CancellationToken ct = default);
    }
}
