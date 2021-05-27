using System;
using System.Collections.Generic;
using System.Linq;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Groups;

namespace ChurchManager.DataImporter.Models
{
    public record CellGroupImport
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? IsOnline { get; set; }
        public string ParentGroupName { get; set; }
        public string Church { get; set; }
        public DateTime? StartDate { get; set; }
        public string MeetingDay { get; set; }
        public string MeetingTime { get; set; }

        public static Group ToEntity(CellGroupImport import, IList<Church> churches, IList<Group> groups = null)
        {
            Schedule schedule = null;
            // set weekly schedule for newly created groups
            if (!string.IsNullOrWhiteSpace(import.MeetingDay) &&
                Enum.TryParse(import.MeetingDay, out DayOfWeek meetingDay))
            {
                TimeSpan.TryParse(import.MeetingTime, out var meetingTime);

                var iCalendarContent =
                    Program.CalendarSerializer.SerializeToString(
                        InetCalendarHelper.CalendarWithWeeklyRecurrence(meetingTime));

                schedule = new Schedule
                {
                    Name = import.Name,
                    WeeklyDayOfWeek = meetingDay,
                    WeeklyTimeOfDay = meetingTime,
                    StartDate = import.StartDate,
                    iCalendarContent = iCalendarContent
                };
            }

            return new Group()
            {
                Name = import.Name,
                Description = import.Description,
                Address = import.Address,
                ChurchId = churches.FirstOrDefault(x => x.Name == import.Church)?.Id,
                IsOnline = import.IsOnline,
                StartDate = import.StartDate,
                GroupTypeId = 1,
                ParentGroupId = groups?.FirstOrDefault(x => x.Name == import.ParentGroupName)?.Id,
                Schedule = schedule
            };
        }
    }
}