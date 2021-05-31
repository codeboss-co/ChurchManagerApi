using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ChurchManager.Persistence.Shared;
using Ical.Net.CalendarComponents;

namespace ChurchManager.Domain.Features.Groups
{
    public class Schedule : Entity<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Date that the Schedule becomes effective/active. This property is inclusive, and the schedule will be inactive before this date. 
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets that date that this Schedule expires and becomes inactive. This value is inclusive and the schedule will be inactive after this date.
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the content lines of the iCalendar
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/>representing the  content of the iCalendar.
        /// </value>
        public string iCalendarContent
        {
            get => _iCalendarContent ?? string.Empty;
            set => _iCalendarContent = value;
        }
        private string _iCalendarContent;

        public DayOfWeek? WeeklyDayOfWeek { get; set; }

        public TimeSpan? WeeklyTimeOfDay { get; set; }

        #region Methods

        /// <summary>
        /// Gets the Schedule's iCalender Event.
        /// </summary>
        /// <value>
        /// A <see cref="CalendarEvent"/> representing the iCalendar event for this Schedule.
        /// </value>
        public virtual CalendarEvent GetICalEvent() => InetCalendarHelper.CreateCalendarEvent(iCalendarContent);

        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        public virtual ScheduleType ScheduleType
        {
            get
            {
                if(WeeklyDayOfWeek.HasValue)
                {
                    return ScheduleType.Weekly;
                }

                var calEvent = GetICalEvent();

                if(calEvent != null)
                {
                    if(calEvent.RecurrenceRules.Any())
                    {
                        var frequencyType = calEvent.RecurrenceRules[0].Frequency;

                        return InetCalendarHelper.FromFrequencyType(frequencyType);
                    }
                }

                return ScheduleType.None;
            }
        }

        #endregion
    }
}
