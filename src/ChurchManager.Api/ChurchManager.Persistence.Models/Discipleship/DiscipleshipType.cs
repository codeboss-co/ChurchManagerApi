using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.Discipleship
{
    [Table("DiscipleshipType", Schema = "Discipleship")]
    public class DiscipleshipType : Entity<int>, IAggregateRoot<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
