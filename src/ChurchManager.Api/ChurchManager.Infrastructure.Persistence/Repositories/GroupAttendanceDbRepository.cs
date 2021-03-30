using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Core.Shared;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using ChurchManager.Infrastructure.Persistence.Specifications;
using ChurchManager.Persistence.Models.Groups;
using Convey.CQRS.Queries;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupAttendanceDbRepository : GenericRepositoryAsync<GroupAttendance>, IGroupAttendanceDbRepository
    {
        public GroupAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<GroupAttendanceViewModel>> BrowseGroupAttendance(QueryParameter query, int groupTypeId, int? churchId, bool? withFeedback, DateTime? from, DateTime? to, CancellationToken ct = default)
        {
            // Paging
            var pagedResult = await Queryable()
                .Specify(new BrowseGroupAttendanceSpecification(groupTypeId, churchId, withFeedback, from, to))
                .PaginateAsync(query);

            return PagedResult<GroupAttendanceViewModel>.Create(
                pagedResult.Items.Select(x => new GroupAttendanceViewModel
                {
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
    }
}
