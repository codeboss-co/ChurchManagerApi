using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace People.Infrastructure.Persistence.Model
{
    [Table("Person", Schema = "People")]
    public class Person : IAggregateRoot<int>
    {
        [Key]
        public int Id { get; set; }

        public int? ChurchId { get; set; }

        public string RecordStatus { get; set; }
        public string ConnectionStatus { get; set; }
        public string AgeClassification { get; set; }
        public string Gender { get; set; }
        public bool? IsDeceased { get; set; } = false;
        public DateTime? DeceasedDate { get; set; }
        public DateTime? FirstVisitDate { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string NickName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public int? BirthDay { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthYear { get; set; }

        public string MaritalStatus { get; set; }
        public DateTime? AnniversaryDate { get; set; }

        public string Email { get; set; }
        public bool? IsEmailActive { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new Collection<PhoneNumber>();
        public string CommunicationPreference { get; set; }

        public string PhotoUrl { get; set; }


        public int? FamilyId { get; set; }

        /// <summary>
        /// Gets or sets the giving group id.  If an individual would like their giving to be grouped with the rest of their family,
        /// this will be the id of their family group.  If they elect to contribute on their own, this value will be null.
        /// </summary>
        public Guid? GivingGroupId { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user login id from AWS Cognito
        /// </summary>
        public string UserLoginId { get; set; }

        public int? ViewedCount { get; set; }

        #region Navigation

        public virtual Family Family { get; set; }

        #endregion
    }
}
