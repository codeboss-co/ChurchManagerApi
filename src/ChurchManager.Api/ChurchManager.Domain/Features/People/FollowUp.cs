using System;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.People
{
    [Table("FollowUp")]
    public class FollowUp : AuditableEntity<int>, IAggregateRoot<int>
    {
        public DateTime AssignedDate { get; set; }
        
        /// <summary>
        /// Date a follow up action was taken
        /// </summary>
        public DateTime? ActionDate { get; set; }
        
        public string Type { get; set; }
        
        public int AssignedPersonId { get; set; }
        
        public int PersonId { get; set; }
        
        public string Severity { get; set; } = "Normal"; // Urgent

        public string Note { get; set; }

        /// <summary>
        /// Whether or not additional follow up is required after doing a follow up
        /// </summary>
        public bool? RequiresAdditionalFollowUp { get; set; }

        #region Navigation

        public virtual Person AssignedPerson { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }
}
