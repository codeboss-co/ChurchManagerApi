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
using Microsoft.Extensions.Logging;
using GroupMemberAttendance = ChurchManager.Domain.Features.Groups.GroupMemberAttendance;

namespace ChurchManager.Application.Features.Groups.Services
{
    public class GroupAttendanceAppService : IGroupAttendanceAppService
    {
        public ILogger<GroupAttendanceAppService> Logger { get; }
        private readonly IGroupAttendanceDbRepository _attendanceDbRepository;
        private readonly IGroupDbRepository _groupDb;
        private readonly IGroupsService _service;

        public GroupAttendanceAppService(
            IGroupDbRepository groupDb,
            IGroupsService service,
            IGroupAttendanceDbRepository attendanceDbRepository,
            ILogger<GroupAttendanceAppService> logger)
        {
            Logger = logger;
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
                if (command.DidNotOccur is true)
                {
                    groupAttendance = new GroupAttendance
                    {
                        GroupId = command.GroupId,
                        AttendanceDate = command.AttendanceDate,
                        DidNotOccur = command.DidNotOccur,
                        Notes = command.Notes
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
                        AttendanceCount = attendees.Count(x => x.DidAttend.HasValue && x.DidAttend.Value) + command.FirstTimers.Count(),
                        FirstTimerCount = command.FirstTimers.Count(),
                        NewConvertCount =
                            attendees.Count(x => x.IsNewConvert.HasValue && x.IsNewConvert.Value),
                        ReceivedHolySpiritCount = 
                            attendees.Count(x => x.ReceivedHolySpirit.HasValue && x.ReceivedHolySpirit.Value),
                        Attendees = attendees,
                        Notes = command.Notes,
                        Offering = command.Offering != null ? new Money("ZAR", command.Offering.Value) : null
                    };
                }

                await _attendanceDbRepository.AddAsync(groupAttendance, ct);
                await _attendanceDbRepository.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                Logger.LogError(ex, $"Error adding attendance record for groupId: {command.GroupId}");
                throw;
            }
        }
    }
}