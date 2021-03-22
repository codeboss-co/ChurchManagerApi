using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.People.Discipleship
{
    [Table("Step", Schema = "Discipleship")]

    public class DiscipleshipStep : AuditableEntity<int>
    {
        public int DiscipleshipTypeId { get; set; }
        public int PersonId { get; set; }
        public DateTime CompletionDate { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }

        #region Navigation

        public virtual DiscipleshipType DiscipleshipType { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }
}
