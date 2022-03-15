using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Types;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Commands.NewGroup
{
    public record EditGroupCommand : AddGroupCommand
    {
        public int GroupId { get; set; }
    }

    public class EditGroupHandler : IRequestHandler<EditGroupCommand, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IDateTimeProvider _dateTime;

        public EditGroupHandler(IGroupDbRepository dbRepository, IDateTimeProvider dateTime)
        {
            _dbRepository = dbRepository;
            _dateTime = dateTime;
        }

        public async Task<ApiResponse> Handle(EditGroupCommand command, CancellationToken ct)
        {
            var group = await _dbRepository.Queryable("GroupType", "Schedule")
                .SingleOrDefaultAsync(x => x.Id == command.GroupId, ct);

            var weeklyTimeOfDay = _dateTime.ConvertFromUtc(command.MeetingTime).TimeOfDay;

            //Repeat daily for 5 days
            var rrule = new RecurrencePattern(command.Recurrence);
            var days = rrule.ByDay;

            var e = new CalendarEvent
            {
                Start = command.Start.HasValue
                    ? new CalDateTime(command.Start.Value.Year, command.Start.Value.Month, command.Start.Value.Day,
                        weeklyTimeOfDay.Hours, weeklyTimeOfDay.Minutes, weeklyTimeOfDay.Seconds)
                    : CalDateTime.Today,
                End = command.End.HasValue ? new CalDateTime(command.End.Value) : CalDateTime.Today.AddYears(5),
                RecurrenceRules = new List<RecurrencePattern> { rrule }
            };

            var calendar = new Calendar();
            calendar.Events.Add(e);

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            group.Name = command.Name;
            group.Description = command.Description;
            group.GroupTypeId = command.GroupTypeId;
            group.ChurchId = command.ParentChurchGroup.ChurchId;
            /*group.ParentGroupId = command.ParentChurchGroup.GroupId is DomainConstants.Groups.NoParentGroupId
                ? null
                : command.ParentChurchGroup.GroupId;*/
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
                    WeeklyTimeOfDay = weeklyTimeOfDay,
                    WeeklyDayOfWeek = days.FirstOrDefault()?.DayOfWeek
                };
            }
            else
            {
                schedule.StartDate = command.Start;
                schedule.EndDate = command.End;
                schedule.Name = $"{command.Name} Schedule";
                schedule.iCalendarContent = serializedCalendar;
                schedule.WeeklyTimeOfDay = weeklyTimeOfDay;
                schedule.WeeklyDayOfWeek = days.FirstOrDefault()?.DayOfWeek;
            }

            group.Schedule = schedule;

            await _dbRepository.UpdateAsync(group, ct);

            // Return the model the front is expecting in the tree
            var groupWithChildren = await _dbRepository.GroupWithChildrenAsync(group.Id, 1, ct);
            var editedGroupWithChildren = groupWithChildren.FirstOrDefault() ?? throw new NullReferenceException(nameof(groupWithChildren));
            return new ApiResponse(editedGroupWithChildren);
        }
    }
}