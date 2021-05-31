using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.BrowseGroupAttendance
{
    public record BrowseGroupAttendanceQuery
        : QueryParameter, IRequest<PagedResponse<GroupAttendanceViewModel>>
    {
        public int GroupTypeId { get; set; } = DomainConstants.GROUP_TYPE_CELL_MINISTRY;
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

        public BrowseGroupAttendanceHandler(IGroupAttendanceDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PagedResponse<GroupAttendanceViewModel>> Handle(BrowseGroupAttendanceQuery query,
            CancellationToken ct)
        {
            var results = await _dbRepository.BrowseGroupAttendance(
                query,
                query.GroupTypeId,
                query.ChurchId,
                query.GroupId,
                query.WithFeedBack,
                query.From, query.To, ct);

            return new PagedResponse<GroupAttendanceViewModel>(results);
        }
    }
}