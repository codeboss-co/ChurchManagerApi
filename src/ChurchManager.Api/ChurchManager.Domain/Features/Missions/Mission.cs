using ChurchManager.Domain.Features.People;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Missions
{
    [Table("Mission")]
    public class Mission : AuditableEntity<int>, IAggregateRoot<int>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Type { get; set; } // InReach, OutReach etc

        [MaxLength(200)]
        public string Category { get; set; } // ROSA, Healing Streams etc

        /// <summary>
        /// Gets or sets the icon CSS class.
        /// </summary>
        [MaxLength(100)]
        public string IconCssClass { get; set; } = "heroicons_solid:calendar";

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Optional Person who headed this mission
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Optional Church who headed this mission
        /// </summary>
        public int? ChurchId { get; set; }

        /// <summary>
        /// Optional Group who headed this mission
        /// </summary>
        public int? GroupId { get; set; }

        public Attendance Attendance { get; set; }

        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the Urls of the photos attached for this entity
        /// </summary>
        public List<string> PhotoUrls { get; set; } = new();

        #region Navigation

        public virtual Person Person { get; set; }
        public virtual Church Church { get; set; }
        public virtual Group Group { get; set; }

        #endregion
    }

    [Owned]
    public class Attendance
    {
        public int? AttendanceCount { get; set; }
        public int? FirstTimerCount { get; set; }
        public int? NewConvertCount { get; set; }
        public int? ReceivedHolySpiritCount { get; set; }
    }
}
