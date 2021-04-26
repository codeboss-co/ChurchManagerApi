using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.Discipleship
{
    [Table("DiscipleshipStepDefinition", Schema = "Discipleship")]

    public class DiscipleshipStepDefinition : AuditableEntity<int>, IAggregateRoot<int>
    {
        public int DiscipleshipTypeId { get; set; }
        public int Order { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        #region Navigation

        public virtual DiscipleshipType DiscipleshipType { get; set; }

        #endregion
    }
}
