using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Application.Features.Groups.Commands.NewGroup
{
    public record EditGroupCommand : AddGroupCommand
    {
        public int GroupId { get; set; }
    }

    public class EditGroupHandler : IRequestHandler<EditGroupCommand, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;

        public EditGroupHandler(IGroupDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(EditGroupCommand command, CancellationToken ct)
        {
            var group = await _dbRepository.Queryable("GroupType", "Schedule")
                .SingleOrDefaultAsync(x =>x.Id == command.GroupId, ct);
            
            //Repeat daily for 5 days
            var rrule = new RecurrencePattern(command.Recurrence);
            var days = rrule.ByDay;

            var e = new CalendarEvent
            {
                Start = command.Start.HasValue ? new CalDateTime(command.Start.Value) : CalDateTime.Today,
                End = command.End.HasValue ? new CalDateTime(command.End.Value) : CalDateTime.Today.AddYears(5),
                RecurrenceRules = new List<RecurrencePattern> {rrule}
            };

            var calendar = new Calendar();
            calendar.Events.Add(e);

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            group.Name = command.Name;
            group.Description = command.Description;
            group.GroupTypeId = command.GroupTypeId;
            group.ChurchId = command.ParentChurchGroup.ChurchId;
            group.ParentGroupId = command.ParentChurchGroup.GroupId is DomainConstants.Groups.NoParentGroupId
                ? null
                : command.ParentChurchGroup.GroupId;
            group.Address = command.Address;
            group.IsOnline = command.IsOnline;
            // Update schedule
            var schedule = group.Schedule;
            if (schedule == null)
            {
                schedule = new Schedule
                {
                    StartDate = command.Start,
                    EndDate = command.End,
                    Name = $"{command.Name} Schedule",
                    iCalendarContent = serializedCalendar,
                    WeeklyTimeOfDay = command.MeetingTime.TimeOfDay,
                    WeeklyDayOfWeek = days.FirstOrDefault()?.DayOfWeek
                };
            }
            else
            {
                schedule.StartDate = command.Start;
                schedule.EndDate = command.End;
                schedule.Name = $"{command.Name} Schedule";
                schedule.iCalendarContent = serializedCalendar;
                schedule.WeeklyTimeOfDay = command.MeetingTime.TimeOfDay;
                schedule.WeeklyDayOfWeek = days.FirstOrDefault()?.DayOfWeek;
            }
            group.Schedule = schedule;

            await _dbRepository.UpdateAsync(group, ct);

            return new ApiResponse(group);
        }
    }
}