using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Features.People;
using GroupMemberAttendance = ChurchManager.Domain.Features.Groups.GroupMemberAttendance;

namespace ChurchManager.Application.Features.Groups.Services
{
    public class GroupAttendanceAppService : IGroupAttendanceAppService
    {
        private readonly IGroupAttendanceDbRepository _attendanceDbRepository;
        private readonly IGroupDbRepository _groupDb;
        private readonly IGroupsService _service;

        public GroupAttendanceAppService(
            IGroupDbRepository groupDb,
            IGroupsService service,
            IGroupAttendanceDbRepository attendanceDbRepository)
        {
            _groupDb = groupDb;
            _service = service;
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
                    var groupMemberRoles = await _service.GroupRolesForGroupAsync(command.GroupId, ct);
                    var spec = new GroupWithTypeSpecification(command.GroupId);
                    var group = await _groupDb.GetBySpecAsync(spec, ct);

                    if (group is null)
                    {
                        throw new ArgumentNullException(nameof(group), $"Cannot find group with id: {command.GroupId} - to add group attendance record");
                    }

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
                                    RecordStatus = RecordStatus.Pending,
                                    Person = new Person
                                    {
                                        FullName = new FullName {FirstName = x.FirstName, LastName = x.LastName},
                                        Gender = x.Gender,
                                        AgeClassification = x.AgeClassification,
                                        FirstVisitDate = command.AttendanceDate,
                                        ConnectionStatus = ConnectionStatus.FirstTimer,
                                        RecordStatus = RecordStatus.Pending,
                                        PhoneNumbers = new List<PhoneNumber> {new() {CountryCode = "+27", Number = x.PhoneNumber}},
                                        Source = $"{group.GroupType.Name}",
                                        ChurchId = group.ChurchId
                                    }
                                },
                                AttendanceDate = command.AttendanceDate,
                                DidAttend = true,
                                IsFirstTime = true,
                                IsNewConvert = x.NewConvert,
                                ReceivedHolySpirit = x.ReceivedHolySpirit,
                                Note = x.Note
                            }
                        ).ToList();
                    // Attendees = Members + First Timers
                    var attendees = members.Concat(firstTimers).ToList();

                    groupAttendance = new GroupAttendance
                    {
                        GroupId = command.GroupId,
                        AttendanceDate = command.AttendanceDate,
                        DidNotOccur = command.DidNotOccur,
                        AttendanceCount = attendees.Where(x => x.DidAttend.HasValue).Count(x => x.DidAttend.Value) +
                                          command.FirstTimers.Count(),
                        FirstTimerCount = command.FirstTimers.Count(),
                        NewConvertCount =
                            attendees.Where(x => x.IsNewConvert.HasValue).Count(x => x.IsNewConvert.Value),
                        ReceivedHolySpiritCount = 
                            attendees.Where(x => x.ReceivedHolySpirit.HasValue).Count(x => x.ReceivedHolySpirit.Value),
                        Attendees = attendees,
                        Notes = command.Notes
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