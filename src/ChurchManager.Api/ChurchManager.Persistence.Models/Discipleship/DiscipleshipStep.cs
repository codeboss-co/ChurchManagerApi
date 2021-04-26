using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Models.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.Discipleship
{
    [Table("DiscipleshipStep", Schema = "Discipleship")]

    public class DiscipleshipStep : AuditableEntity<int>
    {
        public int DiscipleshipStepDefinitionId { get; set; }
        public int PersonId { get; set; }
        public DateTime? CompletionDate { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }

        #region Navigation

        public virtual DiscipleshipStepDefinition Definition { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }
}
