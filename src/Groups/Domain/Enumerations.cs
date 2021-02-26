using Codeboss.Types;
using Groups.Persistence.Models;

namespace Domain
{
    /// <summary>
    /// Represents the status of a <see cref="GroupMember"/> in a <see cref="Group"/>.
    /// </summary>
    public class GroupMemberStatus : Enumeration<GroupMemberStatus, string>
    {
        public GroupMemberStatus(string value) => Value = value;

        /// <summary>
        /// The <see cref="GroupMember"/> is not an active member of the <see cref="Group"/>.
        /// </summary>
        public static GroupMemberStatus Inactive = new( "Inactive");


        /// <summary>
        /// The <see cref="GroupMember"/> is an active member of the <see cref="Group"/>.
        /// </summary>
        public static GroupMemberStatus Active = new("Active");


        /// <summary>
        /// The <see cref="GroupMember">GroupMember's</see> membership in the <see cref="Group"/> is pending.
        /// </summary>
        public static GroupMemberStatus Pending = new("Pending");
    }
}