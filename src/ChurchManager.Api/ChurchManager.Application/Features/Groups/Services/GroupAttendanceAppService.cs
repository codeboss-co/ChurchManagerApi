using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.People.Repositories;
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
        private readonly IPersonDbRepository _personDbRepository;
        private readonly IGroupDbRepository _groupDbRepository;
        private readonly IGroupAttendanceDbRepository _attendanceDbRepository;

        public GroupAttendanceAppService(
            IPersonDbRepository personDbRepository,
            IGroupDbRepository groupDbRepository,
            IGroupAttendanceDbRepository attendanceDbRepository)
        {
            _personDbRepository = personDbRepository;
            _groupDbRepository = groupDbRepository;
            _attendanceDbRepository = attendanceDbRepository;
        }

        public async Task RecordAttendanceAsync(GroupAttendanceRecordCommand command, CancellationToken ct = default)
        {
            var transaction = await _attendanceDbRepository.DbContext.Database.BeginTransactionAsync(ct);

            try
            {
                var groupMemberRoles = await _groupDbRepository.GroupRolesForGroupAsync(command.GroupId, ct);

                var members = command.Members
                    .Select(x => new GroupMemberAttendance
                    {
                        GroupId = command.GroupId,
                        GroupMemberId = x.GroupMemberId,
                        AttendanceDate = command.AttendanceDate.Value,
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
                                
                            },
                            AttendanceDate = command.AttendanceDate.Value,
                            DidAttend = true,
                            IsNewConvert = x.NewConvert,
                            ReceivedHolySpirit = x.ReceivedHolySpirit
                        }
                    ).ToList();

                var groupAttendance = new GroupAttendance
                {
                    GroupId = command.GroupId,
                    AttendanceDate = command.AttendanceDate.Value,
                    DidNotOccur = command.DidNotOccur,
                    AttendanceCount = command.DidNotOccur == null
                        ? null
                        : command.Members.Count() + command.FirstTimers.Count(),
                    FirstTimerCount = command.DidNotOccur == null ? null : command.FirstTimers.Count(),
                    NewConvertCount = command.DidNotOccur == null ? null : command.Members.Count(x => x.NewConvert),
                    Attendees = members
                };

                await _attendanceDbRepository.AddAsync(groupAttendance);

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
            
        }
    }
}
