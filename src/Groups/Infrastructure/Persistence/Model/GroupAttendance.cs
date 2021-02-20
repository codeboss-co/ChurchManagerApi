using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    public class GroupAttendance : IAggregateRoot<int>
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Id of the <see cref="Group"/> that the attendance is for.
        /// </summary>
        public int GroupId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool? DidNotOccur { get; set; }
        public int? AttendanceCount { get; set; }
        public int? FirstTimerCount { get; set; }
        public int? NewConvertCount { get; set; }
        public string Notes { get; set; }

        #region Navigation

        public virtual Group Group { get; set; }

        public virtual ICollection<GroupMemberAttendance> Attendees { get; set; } = new Collection<GroupMemberAttendance>();

        #endregion

        /// <summary>
        /// Gets a value indicating whether attendance was entered (based on presence of any attendee records).
        /// </summary>
        public virtual bool AttendanceEntered => Attendees.Any();

        /// <summary>
        /// Gets the number of attendees who attended.
        /// </summary>
        public virtual int DidAttendCount => Attendees.Count(a => a.DidAttend.HasValue && a.DidAttend.Value);

        /// <summary>
        /// Gets the attendance rate.
        /// </summary>
        /// <value>
        /// The attendance rate which is the number of attendance records marked as did attend
        /// divided by the total number of attendance records for this occurrence.
        /// </value>
        public double AttendanceRate
        {
            get
            {
                var totalCount = Attendees.Count;
                if(totalCount > 0)
                {
                    return (double)(DidAttendCount) / (double)totalCount;
                }

                return 0.0d;
            }
        }
    }
}
