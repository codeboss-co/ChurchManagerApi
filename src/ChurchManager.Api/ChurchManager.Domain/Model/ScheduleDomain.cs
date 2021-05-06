using System.Linq;
using ChurchManager.Domain.Features.Groups;
using Ical.Net;
using Ical.Net.CalendarComponents;

namespace ChurchManager.Domain.Model
{
    public class ScheduleDomain
    {
        private readonly Schedule _entity;

        public ScheduleDomain(Schedule entity) => _entity = entity;

        #region Methods

        /// <summary>
        /// Gets the Schedule's iCalender Event.
        /// </summary>
        /// <value>
        /// A <see cref="CalendarEvent"/> representing the iCalendar event for this Schedule.
        /// </value>
        public virtual CalendarEvent GetICalEvent() => InetCalendarHelper.CreateCalendarEvent(_entity.iCalendarContent);

        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        public virtual ScheduleType ScheduleType
        {
            get
            {
                if(_entity.WeeklyDayOfWeek.HasValue)
                {
                    return ScheduleType.Weekly;
                }

                var calEvent = GetICalEvent();

                if(calEvent != null)
                {
                    if(calEvent.RecurrenceRules.Any())
                    {
                        var frequencyType = calEvent.RecurrenceRules[0].Frequency;

                        switch(frequencyType)
                        {
                            case FrequencyType.Daily: return ScheduleType.Daily;
                            case FrequencyType.Weekly: return ScheduleType.Weekly;
                            case FrequencyType.Monthly: return ScheduleType.Monthly;
                        }
                    }
                }

                return ScheduleType.None;
            }
        }

        #endregion
    }

}
