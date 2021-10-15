using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features.Groups.Queries.GroupMemberAttendance
{
    public record GroupMembersAttendanceQuery(int GroupId, ReportPeriodType Period) : IRequest<ApiResponse>;

    public class GroupMembersAttendanceHandler : IRequestHandler<GroupMembersAttendanceQuery, ApiResponse>
    {
        private readonly IGroupMemberAttendanceDbRepository _attendanceDbRepository;

        public GroupMembersAttendanceHandler(IGroupMemberAttendanceDbRepository attendanceDbRepository)
        {
            _attendanceDbRepository = attendanceDbRepository;
        }

        public async Task<ApiResponse> Handle(GroupMembersAttendanceQuery query, CancellationToken ct)
        {
            var spec = new GroupMembersAttendanceAnalysisSpecification(query.GroupId, query.Period);
            var results = await _attendanceDbRepository.ListAsync(spec, ct);

            var groupByMember = results.GroupBy(x => x.GroupMemberId);
            var groupMemberAttendances = groupByMember.Select(@group => new GroupMemberAttendanceAnalysisViewModel
            {
                GroupMemberId = @group.Key,
                AttendanceRecords = @group.Select(x => x.DidAttend).ToArray(),
            });

            var analysis = new GroupMembersAttendanceAnalysisViewModel(results.Count)
            {
                MembersAttendance = groupMemberAttendances,
                AttendanceDates = groupByMember.SelectMany(x => x.Select(y => y.AttendanceDate)).ToArray()
            };


            return new ApiResponse(analysis);
        }
    }
}
