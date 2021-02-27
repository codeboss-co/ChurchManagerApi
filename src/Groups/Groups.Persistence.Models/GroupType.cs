using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace Groups.Persistence.Models
{
    [Table("GroupType", Schema = "Groups")]

    public record GroupType : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string GroupTerm { get; set; } = "Group";
        [MaxLength(50)]
        public string GroupMemberTerm { get; set; } = "Member";
        public bool TakesAttendance { get; set; } = true;
    }
}
