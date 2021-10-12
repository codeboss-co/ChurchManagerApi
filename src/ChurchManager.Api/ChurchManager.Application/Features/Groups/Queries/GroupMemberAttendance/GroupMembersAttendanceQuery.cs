using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.ViewModels;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using MediatR;

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

            var attendance = new List<GroupMemberAttendanceAnalysisViewModel>(results.Count);

            results.ForEach(x =>
            {
                attendance.Add(
                    new()
                    {
                        GroupMemberId = x.GroupMemberId,
                        AttendanceRecords = new List<GroupMemberAttendanceRecord>()
                    });
            });

            var vm = new GroupMembersAttendanceAnalysisViewModel
            {
                MembersAttendance = attendance
            };

            var test = vm.MembersAttendance.GroupBy(x => x.GroupMemberId);



            return new ApiResponse(vm);
        }
    }
}
