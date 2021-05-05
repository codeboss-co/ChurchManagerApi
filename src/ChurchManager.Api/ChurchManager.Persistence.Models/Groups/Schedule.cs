using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.Groups
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
    }
}
