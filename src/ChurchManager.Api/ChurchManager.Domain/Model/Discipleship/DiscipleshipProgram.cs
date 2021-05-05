using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Model.Discipleship
{
    [Table("DiscipleshipProgram")]

    public class DiscipleshipProgram : AuditableEntity<int>, IAggregateRoot<int>
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        #region Navigation

        public virtual ICollection<DiscipleshipStepDefinition> StepDefinitions { get; set; } = new Collection<DiscipleshipStepDefinition>();

        #endregion
    }
}
