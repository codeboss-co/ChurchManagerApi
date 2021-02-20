using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    /// <summary>
    /// Represents a role that a <see cref="GroupMember"/> can have in a <see cref="Group"/>.  In Rock each member of a group has one 
    /// or more <see cref="GroupMemberRole">GroupRoles</see> assigned to them (for instance the leader of a group can have both a leader and member role). Examples
    /// of roles include leader, member, team leader, coach, host, etc.
    /// </summary>
    public class GroupMemberRole : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLeader { get; set; }
    }
}
