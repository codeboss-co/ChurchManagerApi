using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    public class GroupType : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupTerm { get; set; } = "Group";
        public string GroupMemberTerm { get; set; } = "Member";
        public bool TakesAttendance { get; set; }
    }
}
