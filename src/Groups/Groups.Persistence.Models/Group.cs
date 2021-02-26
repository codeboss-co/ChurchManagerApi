using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Churches.Persistence.Models;
using ChurchManager.Shared.Persistence;
using Codeboss.Types;

namespace Groups.Persistence.Models
{
    [Table("Group", Schema = "Groups")]

    public class Group : Entity<int>, IAggregateRoot<int>
    {
        public int? ParentGroupId { get; set; }
        public int GroupTypeId { get; set; }
        public int? ChurchId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

        public int? GroupCapacity { get; set; }

        #region Navigation

        public virtual Group ParentGroup { get; set; }
        public virtual GroupType GroupType { get; set; }
        public virtual Church Church { get; set; }

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
