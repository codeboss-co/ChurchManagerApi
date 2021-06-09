using System;
using System.Collections.Generic;
using System.Linq;
using ChurchManager.Domain.Features.Groups;
using CodeBoss.AspNetCore.CbDateTime;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

/// <summary>
/// https://blog.elmah.io/generate-calendar-in-ical-format-with-net-using-ical-net/
/// https://github.com/rianjs/ical.net/blob/master/src/Ical.Net.CoreUnitTests/DocumentationExamples.cs
/// </summary>
namespace ChurchManager.Infrastructure.Tests
{
    public class ICalendar_Tests
    {
        private static readonly DateTime Now = DateTime.Now;
        private static readonly DateTime Later = Now.AddHours(2);

        private static readonly CalendarSerializer CalendarSerializer = new();

        [Fact]
        public void Test_ical_library()
        {
            var dailyForFiveDays = new RecurrencePattern(FrequencyType.Daily, 1)
            {
                Count = 5
            };

            var calendarEvent = new CalendarEvent
            {
                Summary = "Testing",
                Start = new CalDateTime(Now),
                End = new CalDateTime(Later),
                RecurrenceRules = new List<RecurrencePattern> {dailyForFiveDays}
            };

            var calendar = new Calendar();
            calendar.Events.Add(calendarEvent);

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            var dayAfterTomorrow = DateTime.Now.AddDays(+2);
            var occurrences = calendar.GetOccurrences(Now, dayAfterTomorrow);

            Assert.Equal(1, calendar.Children.Count);
            Assert.Equal(2, occurrences.Count);
            Assert.Same(calendarEvent, calendar.Children[0]);
        }

        [Fact]
        public void Schedule_Basic_Test()
        {
            var today = DateTime.Now;
            var nextMonth = DateTime.Now.AddMonths(1).Month;

            var specificDates = new List<DateTime>
            {
                new(today.Year, nextMonth, 1),
                new(today.Year, nextMonth, 3),
                new(today.Year, nextMonth, 5),
                new(today.Year, nextMonth, 7),
                new(today.Year, nextMonth, 10),
                new(today.Year, nextMonth, 20),
                new(today.Year, nextMonth, 30)
            };

            var recurrenceDates = new PeriodList();

            specificDates.ForEach(x => recurrenceDates.Add(new CalDateTime(x)));

            var firstDate = specificDates.First();

            var calendarSpecificDates = new Calendar
            {
                // Create an event for the first scheduled date (1am-2am), and set the recurring dates.
                Events =
                {
                    new CalendarEvent
                    {
                        DtStart = new CalDateTime(firstDate.Year, firstDate.Month, firstDate.Day, 1, 0, 0),
                        DtEnd = new CalDateTime(firstDate.Year, firstDate.Month, firstDate.Day, 2, 0, 0),
                        DtStamp = new CalDateTime(firstDate.Year, firstDate.Month, firstDate.Day),
                        RecurrenceDates = new List<PeriodList> {recurrenceDates},
                        Sequence = 0
                    }
                }
            };

            /* Test starts here */
            var schedule = new Schedule
            {
                iCalendarContent = CalendarSerializer.SerializeToString(calendarSpecificDates)
            };
        }

        [Fact]
        public void Schedule_CreatedWithSpecificEventTime_EffectiveEndDateIsNull()
        {
            // Create a weekly recurring calendar that has no end date.
            var startDateTime = Now.AddHours(-2);
            var calendar = InetCalendarHelper.CalendarWithWeeklyRecurrence(startDateTime);

            var nextWeekTomorrow = DateTime.Now.AddDays(+8);
            var occurrences = calendar.GetOccurrences(Now, nextWeekTomorrow);

            var schedule = new Schedule
            {
                iCalendarContent = CalendarSerializer.SerializeToString(calendar)
            };

            Assert.Equal(2, occurrences.Count);
        }

        [Fact]
        public void Schedule_Convert_ToFriendlyScheduleText()
        {
            // Create a weekly recurring calendar that has no end date.
            var startDateTime = Now.AddHours(-2);
            var days = new[] { DayOfWeek.Friday, DayOfWeek.Saturday };
            var time = new TimeSpan(14, 0, 0);
            var calendar = InetCalendarHelper.CalendarWithWeeklyRecurrence(startDateTime, meetingTime: time, days: days);
            // Create the schedule based on the calendar
            var schedule = new Schedule
            {
                iCalendarContent = CalendarSerializer.SerializeToString(calendar)
            };

            // Generate friendly text
            var friendlyScheduleText = schedule.ToFriendlyScheduleText(true);

            Assert.Contains("Weekly: Friday,Saturday at ", friendlyScheduleText) ;
        }
    }
}