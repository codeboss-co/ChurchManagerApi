using System;
using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace ChurchManager.Shared.Persistence
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        public TPrimaryKey Id { get; }
        public string RecordStatus { get; set; } = "Active";
        public DateTime? InactiveDateTime { get; set; }

        #region Auditing

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        [MaxLength(255)]
        public string ModifiedBy { get; set; } 

        #endregion
    }
}
