using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Model.Churches;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Model.Groups
{
    [Table("Group")]

    public class Group : AuditableEntity<int>, IAggregateRoot<int>
    {
        public int? ParentGroupId { get; set; }
        public int GroupTypeId { get; set; }
        public int? ChurchId { get; set; }
        public int? ScheduleId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public int? GroupCapacity { get; set; }
        public bool? IsOnline { get; set; }

        #region Navigation

        public virtual Group ParentGroup { get; set; }
        public virtual GroupType GroupType { get; set; }
        public virtual Church Church { get; set; }
        public virtual Schedule Schedule { get; set; }

        /// <summary>
        /// Gets or sets a collection the Groups that are children of this group.
        /// </summary>
        public virtual ICollection<Group> Groups { get; set; } = new Collection<Group>();

        /// <summary>
        /// Gets or sets a collection containing the <see cref="GroupMember">GroupMembers</see> who are associated with the Group.
        /// Note that this does not include Archived GroupMembers
        /// </summary>
        public virtual ICollection<GroupMember> Members { get; set; } = new Collection<GroupMember>();

        /// <summary>
        /// Gets or sets a collection containing the <see cref="GroupFeature">Features</see> this group has e.g. Child Care Provided
        /// </summary>
        public virtual ICollection<GroupFeature> Features { get; set; } = new Collection<GroupFeature>();

        #endregion
    }
}
