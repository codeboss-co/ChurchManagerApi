using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace Churches.Infrastructure.Persistence.Model
{
    [Table("ChurchGroup", Schema = "Churches")]

    public class ChurchGroup : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? InactiveDateTime { get; set; }


        #region Navigation


        /// <summary>
        /// Gets or sets a collection containing the <see cref="Church">Churches</see> who are associated with the ChurchGroup.
        /// Note that this does not include Archived GroupMembers
        /// </summary>
        public virtual ICollection<Church> Churches { get; set; } = new Collection<Church>();

        #endregion
    }
}
