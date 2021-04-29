using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using ChurchManager.Persistence.Shared;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace ChurchManager.Persistence.Models.Groups
{
    public class Schedule : Entity<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the content lines of the iCalendar
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/>representing the  content of the iCalendar.
        /// </value>
        [DataMember]
        public string iCalendarContent
        {
            get => _iCalendarContent ?? string.Empty;

            set
            {
                _getICalEvent = null;
                _iCalendarContent = value;
            }
        }
        private string _iCalendarContent;

        /// <summary>
        /// Gets or sets the Date that the Schedule becomes effective/active. This property is inclusive, and the schedule will be inactive before this date. 
        /// </summary>
        /// <value>
        /// A <see cref="System.DateTime"/> value that represents the date that this Schedule becomes active.
        /// </value>
        [DataMember]
        [Column(TypeName = "Date")]
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary>
        /// Gets or sets that date that this Schedule expires and becomes inactive. This value is inclusive and the schedule will be inactive after this date.
        /// </summary>
        /// <value>
        /// A <see cref="System.DateTime"/> value that represents the date that this Schedule ends and becomes inactive.
        /// </value>
        [DataMember]
        [Column(TypeName = "Date")]
        public DateTime? EffectiveEndDate { get; set; }

        public DayOfWeek? WeeklyDayOfWeek { get; set; }

        public TimeSpan? WeeklyTimeOfDay { get; set; }

        #region Virtual methods

        /// <summary>
        /// Gets the Schedule's iCalender Event.
        /// </summary>
        /// <value>
        /// A <see cref="Ical.Net.Event"/> representing the iCalendar event for this Schedule.
        /// </value>
        public virtual CalendarEvent GetICalEvent()
        {
            if(_getICalEvent == null)
            {
                _getICalEvent = InetCalendarHelper.CreateCalendarEvent(iCalendarContent);
            }

            return _getICalEvent;
        }

        private CalendarEvent _getICalEvent = null;

        #endregion


        #region Methods



        #endregion
    }

    #region Enumerations

    /// <summary>
    /// Schedule Type
    /// </summary>
    [Flags]
    public enum ScheduleType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Weekly
        /// </summary>
        Daily = 1,

        /// <summary>
        /// Custom
        /// </summary>
        Weekly = 2,

        /// <summary>
        /// Custom
        /// </summary>
        Custom = 4,
    }

    #endregion

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

        public static Calendar CalendarWithWeeklyRecurrence(DateTime? startDateTime = null, DateTime ? endDateTime = null, int? occurrenceCount = null)
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

            int startTimeHour = startDateTime?.Hour ?? today.Hour;
            int startTimeMinutes = startDateTime?.Minute ?? today.Minute;

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
    }
}
