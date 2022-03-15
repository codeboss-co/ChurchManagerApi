using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.BrowseGroupAttendance
{
    public record BrowseGroupAttendanceQuery
        : QueryParameter, IRequest<PagedResponse<GroupAttendanceViewModel>>
    {
        public int GroupTypeId { get; set; }
        public int? ChurchId { get; set; }
        public int? GroupId { get; set; }
        public bool WithFeedBack { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class
        BrowseGroupAttendanceHandler : IRequestHandler<BrowseGroupAttendanceQuery,
            PagedResponse<GroupAttendanceViewModel>>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;
        private readonly IGenericDbRepository<GroupType> _groupTypeRepo;

        public BrowseGroupAttendanceHandler(IGroupAttendanceDbRepository dbRepository, IGenericDbRepository<GroupType> groupTypeRepo)
        {
            _dbRepository = dbRepository;
            _groupTypeRepo = groupTypeRepo;
        }

        public async Task<PagedResponse<GroupAttendanceViewModel>> Handle(BrowseGroupAttendanceQuery query,
            CancellationToken ct)
        {
            // TODO: Pass in group type id from so we can support all groups
            var cellGroupType = await _groupTypeRepo.Queryable().FirstOrDefaultAsync(x => x.Name == "Cell", ct);

            var results = await _dbRepository.BrowseGroupAttendance(
                query,
                cellGroupType.Id,
                query.ChurchId,
                query.GroupId,
                query.WithFeedBack,
                query.From, query.To, ct);

            return new PagedResponse<GroupAttendanceViewModel>(results);
        }
    }
}