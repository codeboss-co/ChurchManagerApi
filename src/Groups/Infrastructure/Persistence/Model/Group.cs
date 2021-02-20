
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    public class Group : IAggregateRoot<int>
    {
        [Key]
        public int Id { get; set; }
        public int? ParentGroupId { get; set; }
        public int GroupTypeId { get; set; }
        public int? ChurchId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int? GroupCapacity { get; set; }

        public DateTime? InactiveDateTime { get; set; }

        #region Navigation

        public virtual Group ParentGroup { get; set; }
        public virtual GroupType GroupType { get; set; }

        /// <summary>
        /// Gets or sets a collection the Groups that are children of this group.
        /// </summary>
        public virtual ICollection<Group> Groups { get; set; } = new Collection<Group>();

        /// <summary>
        /// Gets or sets a collection containing the <see cref="GroupMember">GroupMembers</see> who are associated with the Group.
        /// Note that this does not include Archived GroupMembers
        /// </summary>
        public virtual ICollection<GroupMember> Members { get; set; } = new Collection<GroupMember>();

        #endregion
    }
}
