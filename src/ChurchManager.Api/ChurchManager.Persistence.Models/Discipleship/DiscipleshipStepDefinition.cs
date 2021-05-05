using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Persistence.Models.Discipleship
{
    [Table("DiscipleshipStepDefinition")]

    public class DiscipleshipStepDefinition : AuditableEntity<int>, IAggregateRoot<int>
    {
        [Required]
        public int DiscipleshipProgramId { get; set; }
        
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the icon CSS class.
        /// </summary>
        [MaxLength(100)]
        public string IconCssClass { get; set; } = "heroicons-solid:bookmark";

        /// <summary>
        /// Gets or sets a flag indicating if this step type allows multiple step records per person.
        /// </summary>
        public bool AllowMultiple { get; set; } = true;

        #region Navigation

        public virtual DiscipleshipProgram DiscipleshipProgram { get; set; }

        public virtual ICollection<DiscipleshipStep> Steps { get; set; } = new Collection<DiscipleshipStep>();

        #endregion
    }
}
