using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Discipleship
{
    [Table("DiscipleshipStep")]

    public class DiscipleshipStep : AuditableEntity<int>, IAggregateRoot<int>
    {
        [Required]
        public int DiscipleshipStepDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Person"/> that identifies the Person associated with taking this step. This property is required.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> associated with the completion of this step.
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> associated with the start of this step.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> associated with the end of this step.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }

        [MaxLength(200)]
        public string Note { get; set; }


        /// <summary>
        /// Indicates if this step has been completed
        /// </summary>
        [DataMember]
        public virtual bool IsComplete => Status != null && Status.Equals("Completed");

        #region Navigation

        public virtual DiscipleshipStepDefinition Definition { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }
}
