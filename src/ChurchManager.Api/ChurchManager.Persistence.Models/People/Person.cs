using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Models.Churches;
using ChurchManager.Persistence.Models.People.Discipleship;
using ChurchManager.Persistence.Models.People.Notes;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Persistence.Models.People
{
    [Table("Person", Schema = "People")]
    public class Person : Entity<int>, IAggregateRoot<int>
    {
        public int? ChurchId { get; set; }

        public string ConnectionStatus { get; set; }
        public DeceasedStatus DeceasedStatus { get; set; }
        public string AgeClassification { get; set; }
        public string Gender { get; set; }
        public DateTime? FirstVisitDate { get; set; }

        public FullName FullName { get; set; }
        public BirthDate BirthDate { get; set; }
        public Baptism BaptismStatus { get; set; }

        public string MaritalStatus { get; set; }
        public DateTime? AnniversaryDate { get; set; }

        public Email Email { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new Collection<PhoneNumber>();
        public string CommunicationPreference { get; set; }

        public string PhotoUrl { get; set; }
        public string Occupation { get; set; }

        public int? FamilyId { get; set; }
        public bool? ReceivedHolySpirit { get; set; } = false;

        /// <summary>
        /// Gets or sets the giving group id.  If an individual would like their giving to be grouped with the rest of their family,
        /// this will be the id of their family group.  If they elect to contribute on their own, this value will be null.
        /// </summary>
        public Guid? GivingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the user login id from AWS Cognito
        /// </summary>
        public string UserLoginId { get; set; }

        public int? ViewedCount { get; set; }
        
        /// <summary>
        /// Where this person came from e.g. Cell, Outreach, Church, Online etc
        /// </summary>
        public string Source { get; set; }


        #region Navigation

        public virtual Family Family { get; set; }
        public virtual Church Church { get; set; }
        public virtual ICollection<DiscipleshipStep> DiscipleshipSteps { get; set; } = new Collection<DiscipleshipStep>();
        public virtual ICollection<Note> Notes { get; set; } = new Collection<Note>();
        
        #endregion
    }


    [Owned]
    public class DeceasedStatus
    {
        public bool? IsDeceased { get; set; } = false;
        public DateTime? DeceasedDate { get; set; }
    }

    [Owned]
    public class FullName
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string NickName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
    }

    [Owned]
    public class BirthDate
    {
        public int? BirthDay { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthYear { get; set; }
    }

    [Owned]
    public class Baptism
    {
        public bool? IsBaptised { get; set; }
        public DateTime? BaptismDate { get; set; }
    }

    [Owned]
    public class Email
    {
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}
