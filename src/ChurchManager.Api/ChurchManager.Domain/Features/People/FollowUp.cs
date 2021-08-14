using System;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.People
{
    [Table("FollowUp")]
    public class FollowUp : AuditableEntity<int>
    {
        public DateTime AssignedDate { get; set; }
        public DateTime? ActionDate { get; set; }
        public string Type { get; set; }
        public int AssignedPersonId { get; set; }
        public int PersonId { get; set; }
        public string Severity { get; set; } = "Normal"; // Urgent
        public string Note { get; set; }
        public bool? RequiresAdditionalFollowUp { get; set; }

        #region Navigation

        public virtual Person AssignedPerson { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }
}
