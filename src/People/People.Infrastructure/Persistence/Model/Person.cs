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

        public string RecordStatus { get; set; }
        public string ConnectionStatus { get; set; }
        public string AgeClassification { get; set; }
        public string Gender { get; set; }
        public bool IsDeceased { get; set; } = false;
        public DateTime? DeceasedDate { get; set; }

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

        public int ChurchId { get; set; }

        public int? FamilyId { get; set; }

        /// <summary>
        /// Gets or sets the giving group id.  If an individual would like their giving to be grouped with the rest of their family,
        /// this will be the id of their family group.  If they elect to contribute on their own, this value will be null.
        /// </summary>
        public Guid? GivingGroupId { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public int? ViewedCount { get; set; }

        #region Navigation

        public virtual Family Family { get; set; }

        #endregion
    }

    #region Enumerations

    /// <summary>
    /// Represents the communication preference of a <see cref="CommunicationType"/> in a <see cref="Person"/>.
    /// </summary>
    public class CommunicationType : Enumeration<CommunicationType, string>
    {
        public CommunicationType(string value) => Value = value;

        public static CommunicationType WhatsApp = new("WhatsApp");
        public static CommunicationType Signal = new("Signal");
        public static CommunicationType Email = new("Email");
        public static CommunicationType SMS = new("SMS");
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="AgeClassification"/> in a <see cref="Person"/>.
    /// </summary>
    public class AgeClassification : Enumeration<AgeClassification, string>
    {
        public AgeClassification(string value) => Value = value;

        public static AgeClassification Adult = new("Adult");
        public static AgeClassification Child = new("Child");
        public static AgeClassification Unknown = new("Unknown");
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="Gender"/> in a <see cref="Person"/>.
    /// </summary>
    public class Gender : Enumeration<Gender, string>
    {
        private Gender() { }

        public static Gender Male = new() { Value = "Male" };
        public static Gender Female = new() { Value = "Female" };
        public static Gender Unknown = new() { Value = "Unknown" };
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="ConnectionStatus"/> in a <see cref="Person"/>.
    /// </summary>
    public class ConnectionStatus : Enumeration<ConnectionStatus, string>
    {
        private ConnectionStatus() { }

        public static ConnectionStatus FirstTimer = new() { Value = "First Timer" };
        public static ConnectionStatus NewConvert = new() { Value = "New Convert" };
        public static ConnectionStatus Member = new() { Value = "Member" };
    }

    public class RecordStatus : Enumeration<RecordStatus, string>
    {
        public static RecordStatus Active = new() { Value = "Active" };
        public static RecordStatus InActive = new() { Value = "InActive" };
        public static RecordStatus Pending = new() { Value = "Pending" };
    }

    #endregion
}
