using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeBoss.Extensions;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace ChurchManager.Domain.Features.Groups
{
    /// <summary>
    /// A helper class for processing iCalendar (RFC 5545) schedules.
    /// </summary>
    /// <remarks>
    /// This class uses the iCal.Net implementation of the iCalendar (RFC 5545) standard.
    /// </remarks>
    public static class InetCalendarHelper
    {
        /// <summary>
        /// Creates the calendar event.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <returns></returns>
        public static CalendarEvent CreateCalendarEvent(string iCalendarContent)
        {
            var stringReader = new StringReader(iCalendarContent);
            var calendarList = Calendar.Load(stringReader);
            CalendarEvent calendarEvent = null;

            //// iCal is stored as a list of Calendar's each with a list of Events, etc.  
            //// We just need one Calendar and one Event
            if(calendarList.Events.Count > 0)
            {
                var calendar = calendarList.Calendar;
                if(calendar != null)
                {
                    calendarEvent = calendar.Events[0];
                }
            }

            return calendarEvent;
        }

        /// <summary>
        /// Returns weekly iCal formatted meeting content
        /// </summary>
        /// <returns> FREQ=WEEKLY;BYDAY=MO;INTERVAL=1;UNTIL=20200515T220000Z </returns>
        public static Calendar CalendarWithWeeklyRecurrence(
            DateTime? startDateTime = null,
            DateTime? endDateTime = null,
            TimeSpan? meetingTime = null,
            DayOfWeek[] days = null,
            int? occurrenceCount = null)
        {
            var today = DateTime.UtcNow;

            // Repeat weekly from today until the specified end date.
            var pattern = "RRULE:FREQ=WEEKLY";

            if(days != null)
            {
                pattern += $";BYDAY={days.ToList().AsDelimited(",")}";
            }

            if(endDateTime != null)
            {
                pattern += $";UNTIL={endDateTime:yyyyMMdd}";
            }

            if(occurrenceCount != null)
            {
                pattern += $";COUNT={occurrenceCount}";
            }

            var recurrencePattern = new RecurrencePattern(pattern);

            int startTimeHour = meetingTime?.Hours ?? today.Hour;
            int startTimeMinutes = meetingTime?.Minutes ?? today.Minute;

            var calendar = new Calendar
            {
                Events = { new CalendarEvent
                    {
                        Summary = "Weekly Meeting",
                        Start = new CalDateTime( startDateTime?.Year ?? today.Year, startDateTime?.Month ?? today.Month, startDateTime?.Day ?? today.Day, startTimeHour, startTimeMinutes, 0 ),
                        End = new CalDateTime( startDateTime?.Year ?? today.Year, startDateTime?.Month ?? today.Month, startDateTime?.Day ?? today.Day, startTimeHour + 2, startTimeMinutes, 0 ),
                        RecurrenceRules = new List<RecurrencePattern> { recurrencePattern }
                    }
                }
            };

            return calendar;
        }

        public static Calendar CalendarWithWeeklyRecurrence(TimeSpan? meetingTime, DayOfWeek[] days = null, int ? occurrenceCount = null)
        {
            return CalendarWithWeeklyRecurrence(null, null, meetingTime, days, occurrenceCount);
        }

        /// <summary>
        /// Gets the occurrences for the specified iCal
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <returns></returns>
        public static IList<Occurrence> GetOccurrences(string iCalendarContent, DateTime startDateTime)
        {
            return GetOccurrences(iCalendarContent, startDateTime, null);
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public static IList<Occurrence> GetOccurrences(string iCalendarContent, DateTime startDateTime, DateTime? endDateTime)
        {
            return GetOccurrences(iCalendarContent, startDateTime, endDateTime, null);
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <param name="scheduleStartDateTimeOverride">The schedule start date time override.</param>
        /// <returns></returns>
        public static IList<Occurrence> GetOccurrences(string iCalendarContent, DateTime startDateTime, DateTime? endDateTime, DateTime? scheduleStartDateTimeOverride)
        {
            Occurrence[] occurrenceList = LoadOccurrences(iCalendarContent, startDateTime, endDateTime, scheduleStartDateTimeOverride);

            return occurrenceList;
        }

        /// <summary>
        /// Loads the occurrences.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <param name="scheduleStartDateTimeOverride">The schedule start date time override.</param>
        /// <returns></returns>
        private static Occurrence[] LoadOccurrences(string iCalendarContent, DateTime startDateTime, DateTime? endDateTime, DateTime? scheduleStartDateTimeOverride)
        {
            var iCalEvent = CreateCalendarEvent(iCalendarContent);
            if(iCalEvent == null)
            {
                return new Occurrence[0];
            }

            if(scheduleStartDateTimeOverride.HasValue)
            {
                iCalEvent.DtStart = new CalDateTime(scheduleStartDateTimeOverride.Value);
            }

            if(endDateTime.HasValue)
            {
                return iCalEvent.GetOccurrences(startDateTime, endDateTime.Value).ToArray();
            }

            return iCalEvent.GetOccurrences(startDateTime).ToArray();
        }


        public static ScheduleType FromFrequencyType(FrequencyType frequency) => frequency switch
        {
            FrequencyType.None => ScheduleType.None,
            FrequencyType.Daily => ScheduleType.Daily,
            FrequencyType.Weekly => ScheduleType.Weekly,
            FrequencyType.Monthly => ScheduleType.Monthly,
            _ => ScheduleType.Custom
        };
    }
}
