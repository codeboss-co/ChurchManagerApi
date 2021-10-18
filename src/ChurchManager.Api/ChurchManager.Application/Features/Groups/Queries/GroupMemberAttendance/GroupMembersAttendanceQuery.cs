using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Shared;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Application.Features.Groups.Queries.GroupMemberAttendance
{
    public record GroupMembersAttendanceQuery(int GroupId, PeriodType Period) : IRequest<ApiResponse>;

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

            var groupByMember = results
                .GroupBy(x => new { x.GroupMemberId , x.GroupMember.PersonId, FullName = x.GroupMember.Person.FullName.ToString()})
                .ToList();

            var groupMemberAttendances = groupByMember.Select(@group => new GroupMemberAttendanceAnalysisViewModel
            {
                GroupMemberId = @group.Key.GroupMemberId,
                PersonId = @group.Key.PersonId,
                PersonName = @group.Key.FullName,
                AttendanceRecords = @group.Select(x => x.DidAttend).ToArray(),
            });

            var analysis = new GroupMembersAttendanceAnalysisViewModel(results.Count)
            {
                MembersAttendance = groupMemberAttendances,
                // So we can map attendance array to attendance date header
                AttendanceDates = groupByMember.SelectMany(x => x.Select(y => y.AttendanceDate))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray()
            };


            return new ApiResponse(analysis);
        }
    }

    /*
     * -------------------------------------------------------------------------------------------------------------
     */

    public record GroupAttendanceQuery(int GroupId, PeriodType Period) : IRequest<ApiResponse>;

    public class GroupAttendance2Handler : IRequestHandler<GroupAttendanceQuery, ApiResponse>
    {
        private readonly IGroupAttendanceDbRepository _attendanceDbRepository;

        public GroupAttendance2Handler(IGroupAttendanceDbRepository attendanceDbRepository)
        {
            _attendanceDbRepository = attendanceDbRepository;
        }

        public async Task<ApiResponse> Handle(GroupAttendanceQuery query, CancellationToken ct)
        {
            var spec = new GroupAttendancesByGroupSpecification(query.GroupId, query.Period);
            var results = await _attendanceDbRepository.ListAsync<GroupAttendanceViewModel>(spec, ct);

            var attendances = results
                .Select(x => new {x.AttendanceDate, x.Attendees})
                .ToList();

            var groupByMember = attendances.SelectMany(x => x.Attendees)
                .GroupBy(x => new { x.GroupMemberId, x.GroupMember.PersonId, x.GroupMember.FirstName, x.GroupMember.LastName })
                .ToList();

            var groupMemberAttendances = groupByMember.Select(@group => new GroupMemberAttendanceAnalysisViewModel
            {
                GroupMemberId = @group.Key.GroupMemberId,
                PersonId = @group.Key.PersonId,
                PersonName = $"{@group.Key.FirstName} {@group.Key.LastName}",
                AttendanceRecords = @group.Select(x => x.DidAttend).Take(10).ToArray()
            });

            var analysis = new GroupMembersAttendanceAnalysisViewModel(results.Count)
            {
                MembersAttendance = groupMemberAttendances,
                // So we can map attendance array to attendance date header
                AttendanceDates = groupByMember.SelectMany(x => x.Select(y => y.AttendanceDate))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray(),

                AvgAttendanceRate = results.Average(x => x.AttendanceRate)
            };

            return new ApiResponse(analysis);
        }
    }
}
