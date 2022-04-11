using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Types;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using MediatR;

namespace ChurchManager.Features.Groups.Commands.NewGroup
{
    public record AddGroupCommand : IRequest<ApiResponse>
    {
        public int GroupTypeId { get; set; }
        public ParentChurchGroup ParentChurchGroup { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? IsOnline { get; set; }
        public DateTime MeetingTime { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Recurrence { get; set; }
    }

    public record ParentChurchGroup
    {
        public int ChurchId { get; set; }
        public int? GroupId { get; set; }
    }

    public class NewGroupHandler : IRequestHandler<AddGroupCommand, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;
        private readonly IDateTimeProvider _dateTime;

        public NewGroupHandler(IGroupDbRepository dbRepository, IDateTimeProvider dateTime)
        {
            _dbRepository = dbRepository;
            _dateTime = dateTime;
        }

        public async Task<ApiResponse> Handle(AddGroupCommand command, CancellationToken ct)
        {
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
                RecurrenceRules = new List<RecurrencePattern> {rrule}
            };

            var calendar = new Calendar();
            calendar.Events.Add(e);

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            // 
            var parentGroupId = command.ParentChurchGroup.GroupId is DomainConstants.Groups.NoParentGroupId
                ? null
                : command.ParentChurchGroup.GroupId;

            var group = new Group
            {
                Name = command.Name, Description = command.Description,
                GroupTypeId = command.GroupTypeId,
                ChurchId = command.ParentChurchGroup.ChurchId,
                ParentGroupId = parentGroupId,
                Address = command.Address,
                IsOnline = command.IsOnline,
                StartDate = DateTimeOffset.UtcNow,
                Schedule = new Schedule
                {
                    StartDate = command.Start,
                    EndDate = command.End,
                    Name = $"{command.Name} Schedule",
                    iCalendarContent = serializedCalendar,
                    WeeklyTimeOfDay = weeklyTimeOfDay,
                    WeeklyDayOfWeek = days.FirstOrDefault()?.DayOfWeek
                }
            };

            await _dbRepository.AddAsync(group, ct);

            return new ApiResponse(group.Id);
        }
    }
}