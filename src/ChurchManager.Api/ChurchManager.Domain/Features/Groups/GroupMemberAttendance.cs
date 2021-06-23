using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Groups
{
    /// <summary>
    /// Represents an instance where a <see cref="GroupMember"/> attended (or was scheduled to attend) a group/location/schedule.
    /// This can be used for attendee/volunteer check-in, group attendance, etc.
    /// </summary>
    [Table("GroupMemberAttendance")]

    public class GroupMemberAttendance : Entity<int>, IAggregateRoot<int>
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="GroupMember"/> that attended/checked in to the <see cref="Group"/>
        /// </summary>
        public int GroupMemberId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Group"/> that the individual attended/checked in to.
        /// </summary>
        public int GroupId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public bool? DidAttend { get; set; } = true;

        public bool? IsFirstTime { get; set; }

        public bool? IsNewConvert { get; set; }
        public bool? ReceivedHolySpirit { get; set; }
        [MaxLength(200)]
        public string Note { get; set; }

        #region Navigation

        public virtual Group Group { get; set; }
        public virtual GroupMember GroupMember { get; set; }

        #endregion
    }
}
