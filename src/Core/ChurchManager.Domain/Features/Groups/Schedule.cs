using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ChurchManager.Persistence.Shared;
using CodeBoss.Extensions;
using Codeboss.Types;
using Ical.Net;
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

        /// <summary>
        /// Gets the Friendly Text of the Calendar Event.
        /// For example, "Every 3 days at 10:30am", "Monday, Wednesday, Friday at 5:00pm", "Saturday at 4:30pm"
        /// </summary>
        /// <param name="condensed">if set to <c>true</c> [condensed].</param>
        /// <returns>
        /// A <see cref="System.String" /> containing a friendly description of the Schedule.
        /// </returns>
        public string ToFriendlyScheduleText(bool condensed)
        {
            // init the result to just the schedule name just in case we can't figure out the FriendlyText
            string result = Name;

            var calendarEvent = GetICalEvent();
            if(calendarEvent != null && calendarEvent.DtStart != null)
            {
                string startTimeText = calendarEvent.DtStart.Value.TimeOfDay.ToString(@"hh\:mm");
                if(calendarEvent.RecurrenceRules.Any())
                {
                    // some type of recurring schedule
                    var rrule = calendarEvent.RecurrenceRules[0];
                    switch(rrule.Frequency)
                    {
                        case FrequencyType.Daily:
                            result = "Daily";

                            if(rrule.Interval > 1)
                            {
                                result += $" every {rrule.Interval} days";
                            }

                            result += " at " + startTimeText;

                            break;

                        case FrequencyType.Weekly:

                            result = rrule.ByDay.Select(a => a.DayOfWeek.ConvertToString()).ToList().AsDelimited(",");
                            if(string.IsNullOrEmpty(result))
                            {
                                // no day selected, so it has an incomplete schedule
                                return "No Scheduled Days";
                            }

                            if(rrule.Interval > 1)
                            {
                                result = $"Every {rrule.Interval} weeks: " + result;
                            }
                            else
                            {
                                result = "Weekly: " + result;
                            }

                            result += " at " + startTimeText;

                            break;

                        case FrequencyType.Monthly:

                            if(rrule.ByMonthDay.Count > 0)
                            {
                                // Day X of every X Months (we only support one day in the ByMonthDay list)
                                int monthDay = rrule.ByMonthDay[0];
                                result = $"Day {monthDay} of every ";
                                if(rrule.Interval > 1)
                                {
                                    result += $"{rrule.Interval} months";
                                }
                                else
                                {
                                    result += "month";
                                }

                                result += " at " + startTimeText;
                            }
                            else if(rrule.ByDay.Count > 0)
                            {
                                // The Nth <DayOfWeekName>.  We only support one *day* in the ByDay list, but multiple *offsets*.
                                // So, it can be the "The first and third Monday" of every month.
                                var bydate = rrule.ByDay[0];
                                var offsetNames = NthNamesAbbreviated.Where(a => rrule.ByDay.Select(o => o.Offset).Contains(a.Key)).Select(a => a.Value);
                                if(offsetNames != null)
                                {
                                    result = $"The {offsetNames.JoinStringsWithCommaAnd()} {bydate.DayOfWeek.ConvertToString()} of every month";
                                }
                                else
                                {
                                    // unsupported case (just show the name)
                                }

                                result += " at " + startTimeText;
                            }
                            else
                            {
                                // unsupported case (just show the name)
                            }

                            break;

                        default:
                            // some other type of recurring type (probably specific dates).  Just return the Name of the schedule
                            break;
                    }
                }
                else
                {
                    // not any type of recurring, might be one-time or from specific dates, etc
                    var dates = InetCalendarHelper.GetOccurrences(iCalendarContent, DateTime.MinValue, DateTime.MaxValue, null)
                        .Where(a => a.Period != null && a.Period.StartTime != null)
                        .Select(a => a.Period.StartTime.Value)
                        .OrderBy(a => a).ToList();

                    if(dates.Count() > 1)
                    {
                        if(condensed || dates.Count() > 99)
                        {
                            result = $"Multiple dates between {dates.First().ToShortDateString()} and {dates.Last().ToShortDateString()}";
                        }
                        else
                        {
                            var listHtml = "<ul class='list-unstyled'>" + Environment.NewLine;
                            foreach(var date in dates)
                            {
                                listHtml += $"<li>{date.ToShortDateTimeString()}</li>" + Environment.NewLine;
                            }

                            listHtml += "</ul>";

                            result = listHtml;
                        }
                    }
                    else if(dates.Count() == 1)
                    {
                        result = "Once at " + calendarEvent.DtStart.Value.ToShortDateTimeString();
                    }
                    else
                    {
                        return "No Schedule";
                    }
                }
            }
            else
            {
                if(WeeklyDayOfWeek.HasValue)
                {
                    result = WeeklyDayOfWeek.Value.ConvertToString();
                    if(WeeklyTimeOfDay.HasValue)
                    {
                        result += " at " + WeeklyTimeOfDay.Value.ToString(@"hh\:mm");
                    }
                }
                else
                {
                    // no start time.  Nothing scheduled
                    return "No Schedule";
                }
            }

            return result;
        }

        #endregion

        #region Constants

        /// <summary>
        /// The "nth" names for DayName of Month (First, Second, Third, Forth, Last)
        /// </summary>
        public static readonly Dictionary<int, string> NthNames = new Dictionary<int, string> {
            { 1, "First" },
            { 2, "Second" },
            { 3, "Third" },
            { 4, "Fourth" },
            { -1, "Last" }
        };

        /// <summary>
        /// The abbreviated "nth" names for DayName of Month (1st, 2nd, 3rd, 4th, last)
        /// </summary>
        private static readonly Dictionary<int, string> NthNamesAbbreviated = new Dictionary<int, string> {
            { 1, "1st" },
            { 2, "2nd" },
            { 3, "3rd" },
            { 4, "4th" },
            { -1, "last" }
        };

        #endregion
    }
}
