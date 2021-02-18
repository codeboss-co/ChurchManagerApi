using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    public class GroupMember : IEntity<int>
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Person"/> that is represented by the GroupMember. This property is required.
        /// </summary>
        public int PersonId { get; set; }

        public int GroupRoleId { get; set; }

        public string GroupMemberStatus { get; set; } = "Active";
    }

    #region Enumerations

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

    #endregion


}
