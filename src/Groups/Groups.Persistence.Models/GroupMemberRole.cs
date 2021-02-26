using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace Groups.Persistence.Models
{
    /// <summary>
    /// Represents a role that a <see cref="GroupMember"/> can have in a <see cref="Group"/>.  In Rock each member of a group has one 
    /// or more <see cref="GroupMemberRole">GroupRoles</see> assigned to them (for instance the leader of a group can have both a leader and member role). Examples
    /// of roles include leader, member, team leader, coach, host, etc.
    /// </summary>
    [Table("GroupMemberRole", Schema = "Groups")]

    public class GroupMemberRole : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public bool IsLeader { get; set; }
    }
}
