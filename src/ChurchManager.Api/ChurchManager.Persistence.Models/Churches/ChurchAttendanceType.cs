using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.Churches
{
    public record ChurchAttendanceType : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
