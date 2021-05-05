using System;
using System.Collections.Generic;
using System.IO;
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

        public static Calendar CalendarWithWeeklyRecurrence(DateTime? startDateTime = null, DateTime? endDateTime = null, TimeSpan? meetingTime = null, int? occurrenceCount = null)
        {
            var today = DateTime.UtcNow;

            // Repeat weekly from today until the specified end date.
            var pattern = "RRULE:FREQ=WEEKLY";

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

        public static Calendar CalendarWithWeeklyRecurrence(TimeSpan? meetingTime, int? occurrenceCount = null)
        {
            return CalendarWithWeeklyRecurrence(null, null, meetingTime, occurrenceCount);
        }
    }
}
