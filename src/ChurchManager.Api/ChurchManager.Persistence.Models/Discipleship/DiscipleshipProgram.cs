using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.Discipleship
{
    [Table("DiscipleshipProgram", Schema = "Discipleship")]

    public class DiscipleshipProgram : AuditableEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        #region Navigation

        public virtual ICollection<DiscipleshipStep> DiscipleshipSteps { get; set; } = new Collection<DiscipleshipStep>();

        #endregion
    }
}
