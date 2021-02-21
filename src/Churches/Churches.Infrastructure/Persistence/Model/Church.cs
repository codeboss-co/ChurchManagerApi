using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace Churches.Infrastructure.Persistence.Model
{
    [Table("Church", Schema = "Churches")]

    public class Church : IAggregateRoot<int>
    {
        [Key]

        public int Id { get; set; }

        public int? ChurchGroupId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? InactiveDateTime { get; set; }

        #region Navigation

        /// <summary>
        /// Gets or sets  the <see cref="ChurchGroup">ChurchGroup</see> that this Church may belong to
        /// Note that this does not include Archived GroupMembers
        /// </summary>
        public virtual ChurchGroup ChurchGroup { get; set; }

        #endregion
    }
}
