using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Models.People;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.Groups
{
    [Table("GroupMember")]

    public class GroupMember : Entity<int>, IAggregateRoot<int>
    {
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Person"/> that is represented by the GroupMember. This property is required.
        /// </summary>
        public int PersonId { get; set; }

        public int GroupRoleId { get; set; }

        public DateTime? FirstVisitDate { get; set; }

        public ArchiveStatus ArchiveStatus { get; set; }

        /// <summary>
        /// Gets or sets the communication preference.
        /// </summary>
        [MaxLength(20)]
        public string CommunicationPreference { get; set; } = "Email";

        #region Navigation
        public virtual Group Group { get; set; }
        public virtual GroupTypeRole GroupRole { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }

    #region Owned

    [Owned]
    public class ArchiveStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether this group member is archived (soft deleted)
        /// </summary>
        public bool? IsArchived { get; set; } = false;

        /// <summary>
        /// Gets or sets the date time that this group member was archived (soft deleted)
        /// </summary>
        /// <value>
        public DateTime? ArchivedDateTime { get; set; }
    }

    #endregion
}
