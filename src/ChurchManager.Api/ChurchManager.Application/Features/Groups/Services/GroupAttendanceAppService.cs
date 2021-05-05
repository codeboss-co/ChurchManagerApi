using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Models.Groups;
using ChurchManager.Persistence.Models.People;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGroupAttendanceDbRepository _attendanceDbRepository;

        public GroupAttendanceAppService(
            IGroupDbRepository groupDbRepository,
            IGroupAttendanceDbRepository attendanceDbRepository)
        {
            _groupDbRepository = groupDbRepository;
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
                    var group = await _groupDbRepository
                        .Queryable()
                        .AsNoTracking()
                        .Include(x => x.GroupType)
                        .SingleAsync(x => x.Id == command.GroupId, ct);

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
                                GroupId = command.GroupId,
                                GroupRole = groupMemberRoles.First(x => !x.IsLeader),
                                Person = new Person
                                {
                                    FullName = new FullName { FirstName = x.FirstName, LastName = x.LastName},
                                    Gender = x.Gender,
                                    FirstVisitDate = command.AttendanceDate,
                                    ConnectionStatus = ConnectionStatus.FirstTimer,
                                    RecordStatus = RecordStatus.Pending,
                                    PhoneNumbers = new List<PhoneNumber>() { new() { CountryCode = "+27" , Number = x.PhoneNumber } },
                                    Source = $"{group.GroupType.Name} {group.GroupType.GroupTerm}"
                                }
                            },
                            AttendanceDate = command.AttendanceDate,
                            DidAttend = true,
                            IsFirstTime = true,
                            IsNewConvert = x.NewConvert,
                            ReceivedHolySpirit = x.ReceivedHolySpirit
                        }
                        ).ToList();

                    var attendees = members.Concat(firstTimers).ToList();

                    groupAttendance = new GroupAttendance
                    {
                        GroupId = command.GroupId,
                        AttendanceDate = command.AttendanceDate,
                        DidNotOccur = command.DidNotOccur,
                        AttendanceCount = attendees.Where(x => x.DidAttend.HasValue).Count(x => x.DidAttend.Value) + command.FirstTimers.Count(),
                        FirstTimerCount = command.FirstTimers.Count(),
                        NewConvertCount = attendees.Where(x =>x.IsNewConvert.HasValue).Count(x => x.IsNewConvert.Value),
                        ReceivedHolySpiritCount = attendees.Where(x => x.ReceivedHolySpirit.HasValue).Count(x => x.ReceivedHolySpirit.Value),
                        Attendees = attendees
                    };
                }

                await _attendanceDbRepository.AddAsync(groupAttendance);
                await _attendanceDbRepository.SaveChangesAsync();
                await transaction.CommitAsync(ct);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
