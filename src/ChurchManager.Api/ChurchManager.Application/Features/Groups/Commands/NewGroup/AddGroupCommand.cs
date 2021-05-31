using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Wrappers;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Commands.NewGroup
{
    public record AddGroupCommand : IRequest<ApiResponse>
    {
        public int ChurchId { get; set; }
        public int GroupTypeId { get; set; }
        public int? ParentGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? IsOnline { get; set; }
        public DateTime MeetingTime { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Recurrence { get; set; }
    }

    public class NewGroupHandler : IRequestHandler<AddGroupCommand, ApiResponse>
    {
        private readonly IGroupDbRepository _dbRepository;

        public NewGroupHandler(IGroupDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(AddGroupCommand command, CancellationToken ct)
        {
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

            var group = new Group
            {
                Name = command.Name, Description = command.Description,
                ChurchId = command.ChurchId,
                GroupTypeId = command.GroupTypeId,
                ParentGroupId = command.ParentGroupId,
                Address = command.Address,
                IsOnline = command.IsOnline,
                StartDate = DateTimeOffset.UtcNow,
                Schedule = new Schedule
                {
                    StartDate = command.Start,
                    EndDate = command.End,
                    Name = $"{command.Name} Schedule",
                    iCalendarContent = serializedCalendar,
                    WeeklyTimeOfDay = command.MeetingTime.TimeOfDay,
                    WeeklyDayOfWeek = days.FirstOrDefault()?.DayOfWeek
                }
            };

            await _dbRepository.AddAsync(group, ct);

            return new ApiResponse(group.Id);
        }
    }
}