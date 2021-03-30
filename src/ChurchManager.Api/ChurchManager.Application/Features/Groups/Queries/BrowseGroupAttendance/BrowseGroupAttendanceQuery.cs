using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Core.Shared.Parameters;
using ChurchManager.Domain.Features.Groups.Repositories;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.BrowseGroupAttendance
{
    public record BrowseGroupAttendanceQuery
        : SearchTermQueryParameter, IRequest<PagedResponse<object>>
    {
        public int GroupTypeId { get; set; }
        public int? ChurchId { get; set; }
        public bool? WithFeedBack { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class BrowseGroupAttendanceHandler : IRequestHandler<BrowseGroupAttendanceQuery, PagedResponse<object>>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;

        public BrowseGroupAttendanceHandler(IGroupAttendanceDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<PagedResponse<object>> Handle(BrowseGroupAttendanceQuery query, CancellationToken ct)
        {
            var results = await _dbRepository.BrowseGroupAttendance(query, query.GroupTypeId, query.ChurchId,
                query.WithFeedBack, query.From, query.To, ct);

            return new PagedResponse<object>(results);
        }
    }

}
