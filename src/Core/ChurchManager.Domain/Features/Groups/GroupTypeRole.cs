using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Groups
{
    /// <summary>
    /// Represents a role that a <see cref="GroupMember"/> can have in a <see cref="Group"/>.  In Rock each member of a group has one 
    /// or more <see cref="GroupTypeRole">GroupRoles</see> assigned to them (for instance the leader of a group can have both a leader and member role). Examples
    /// of roles include leader, member, team leader, coach, host, etc.
    /// </summary>
    [Table("GroupRole")]

    public class GroupTypeRole : Entity<int>, IAggregateRoot<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public bool IsLeader { get; set; }

        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanManageMembers { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="GroupType"/> that this GroupRole belongs to. This property is required.
        /// </summary>
        public int? GroupTypeId { get; set; }

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="GroupType"/> that this GroupRole belongs to.
        /// </summary>
        public virtual GroupType GroupType { get; set; }

        #endregion
    }
}
