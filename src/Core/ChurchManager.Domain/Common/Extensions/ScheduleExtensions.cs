using System.Linq;
using ChurchManager.Domain.Features.Groups;

namespace ChurchManager.Domain.Common.Extensions
{
    public static class ScheduleExtensions
    {
        /// <summary>
        /// Adds the INTERVAL value; as by default CalendarEvent does not add it if `Interval=1`
        /// </summary>
        public static string WithInterval(this Schedule schedule)
        {
            if (schedule?.GetICalEvent() == null) return null;

            var calendarEvent = schedule.GetICalEvent();

            var recurrencePattern = calendarEvent.RecurrenceRules.FirstOrDefault();
            var recurrenceRuleText = recurrencePattern?.ToString();

            if (recurrenceRuleText != null && !recurrenceRuleText.Contains("INTERVAL"))
            {
                recurrenceRuleText += $";INTERVAL={recurrencePattern.Interval}";
            }

            return recurrenceRuleText;
        }
    }
}
