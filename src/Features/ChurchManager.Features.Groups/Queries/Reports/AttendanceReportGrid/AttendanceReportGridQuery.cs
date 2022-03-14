using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Queries.Reports.AttendanceReportGrid
{
    public record AttendanceReportGridQuery : IRequest<ApiResponse>
    {
        public int GroupTypeId { get; set; }
        public IList<int> GroupId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class AttendanceReportGridHandler : IRequestHandler<AttendanceReportGridQuery, ApiResponse>
    {
        private readonly IGroupAttendanceDbRepository _dbRepository;

        public AttendanceReportGridHandler(IGroupAttendanceDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(AttendanceReportGridQuery query, CancellationToken ct)
        {
            var spec = new AttendanceReportGridSpecification(query.GroupTypeId, query.GroupId, query.From, query.To);

            var results = await _dbRepository.ListAsync<GroupAttendanceViewModel>(spec, ct);

            return new ApiResponse(results);
        }
    }
}
