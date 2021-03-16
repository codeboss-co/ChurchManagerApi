using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Persistence.Models.Groups;
using GroupMemberAttendance = ChurchManager.Persistence.Models.Groups.GroupMemberAttendance;

namespace ChurchManager.Application.Features.Groups.Services
{
    public interface IGroupAttendanceAppService
    {
        Task RecordAttendanceAsync(GroupAttendanceRecordCommand command, CancellationToken ct = default);
    }

    public class GroupAttendanceAppService : IGroupAttendanceAppService
    {
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IGenericRepositoryAsync<GroupMemberAttendance> _groupMemberAttendance;
        private readonly IGroupAttendanceDbRepository _attendanceDbRepository;

        public GroupAttendanceAppService(
            IGroupDbRepository groupDbRepository,
            IGenericRepositoryAsync<GroupMemberAttendance> groupMemberAttendance,
            IGroupAttendanceDbRepository attendanceDbRepository)
        {
            _groupDbRepository = groupDbRepository;
            _groupMemberAttendance = groupMemberAttendance;
            _attendanceDbRepository = attendanceDbRepository;
        }

        public async Task RecordAttendanceAsync(GroupAttendanceRecordCommand command, CancellationToken ct = default)
        {
            var transaction = await _attendanceDbRepository.DbContext.Database.BeginTransactionAsync(ct);

            try
            {
                GroupAttendance groupAttendance;

                // Group meeting did not occur
                if (command.DidNotOccur.HasValue && command.DidNotOccur == true)
                {
                    groupAttendance = new GroupAttendance
                    {
                        GroupId = command.GroupId,
                        AttendanceDate = command.AttendanceDate,
                        DidNotOccur = command.DidNotOccur
                    };
                }
                else
                {
                    var groupMemberRoles = await _groupDbRepository.GroupRolesForGroupAsync(command.GroupId, ct);

                    var members = command.Members
                        .Select(x => new GroupMemberAttendance
                        {
                            GroupId = command.GroupId,
                            GroupMemberId = x.GroupMemberId,
                            AttendanceDate = command.AttendanceDate,
                            DidAttend = x.GroupMemberDidAttend,
                            IsNewConvert = x.NewConvert,
                            ReceivedHolySpirit = x.ReceivedHolySpirit
                        }
                    ).ToList();

                    var firstTimers = command.FirstTimers
                        .Select(x => new GroupMemberAttendance
                        {
                            GroupId = command.GroupId,
                            GroupMember = new GroupMember
                            {
                                GroupMemberRole = groupMemberRoles.First(x => !x.IsLeader)
                            },
                            AttendanceDate = command.AttendanceDate,
                            DidAttend = true,
                            IsNewConvert = x.NewConvert,
                            ReceivedHolySpirit = x.ReceivedHolySpirit
                        }
                        ).ToList();

                    await _groupMemberAttendance.AddRangeAsync(firstTimers);
                    await _attendanceDbRepository.SaveChangesAsync();

                    var attendees = members.Concat(firstTimers).ToList();

                    groupAttendance = new GroupAttendance
                    {
                        GroupId = command.GroupId,
                        AttendanceDate = command.AttendanceDate,
                        DidNotOccur = command.DidNotOccur,
                        AttendanceCount = command.DidNotOccur == null
                            ? null
                            : command.Members.Count() + command.FirstTimers.Count(),
                        FirstTimerCount = command.DidNotOccur == null ? null : command.FirstTimers.Count(),
                        NewConvertCount = command.DidNotOccur == null ? null : command.Members.Count(x => x.NewConvert),
                        Attendees = members
                    };
                }

                await _attendanceDbRepository.AddAsync(groupAttendance);
                await _attendanceDbRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
