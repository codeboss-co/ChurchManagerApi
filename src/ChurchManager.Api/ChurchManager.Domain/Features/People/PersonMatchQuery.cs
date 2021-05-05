using System;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Model.People;
using CodeBoss.Extensions;

namespace ChurchManager.Application.Features.People.Queries
{
    /// <summary>
    /// Contains the properties that can be searched for when performing a GetBestMatch query
    /// </summary>
    public class PersonMatchQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonMatchQuery"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="mobilePhone">The mobile phone.</param>
        public PersonMatchQuery(string firstName, string lastName, string email, string mobilePhone)
        {
            FirstName = firstName.IsNullOrEmpty() ? firstName.Trim() : string.Empty;
            LastName = lastName.IsNullOrEmpty() ? lastName.Trim() : string.Empty;
            Email = email.IsNullOrEmpty() ? email.Trim() : string.Empty;
            MobilePhone = mobilePhone.IsNullOrEmpty() ? PhoneNumber.CleanNumber(mobilePhone) : string.Empty;
            Gender = null;
            BirthDate = null;
            SuffixValueId = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonMatchQuery" /> class. Use this constructor when the person may not have a birth year.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="mobilePhone">The mobile phone.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="birthMonth">The birth month.</param>
        /// <param name="birthDay">The birth day.</param>
        /// <param name="birthYear">The birth year.</param>
        /// <param name="suffixValueId">The suffix value identifier.</param>
        public PersonMatchQuery(string firstName, string lastName, string email, string mobilePhone, Gender? gender = null, int? birthMonth = null, int? birthDay = null, int? birthYear = null, int? suffixValueId = null)
        {
            FirstName = firstName.IsNullOrEmpty() ? firstName.Trim() : string.Empty;
            LastName = lastName.IsNullOrEmpty() ? lastName.Trim() : string.Empty;
            Email = email.IsNullOrEmpty() ? email.Trim() : string.Empty;
            MobilePhone = mobilePhone.IsNullOrEmpty() ? PhoneNumber.CleanNumber(mobilePhone) : string.Empty;
            Gender = gender;
            BirthDate = birthDay.HasValue && birthMonth.HasValue ? new DateTime(birthYear ?? DateTime.MinValue.Year, birthMonth.Value, birthDay.Value) : (DateTime?)null;
            SuffixValueId = suffixValueId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonMatchQuery"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="mobilePhone">The mobile phone.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="birthDate">The birth date.</param>
        /// <param name="suffixValueId">The suffix value identifier.</param>
        public PersonMatchQuery(string firstName, string lastName, string email, string mobilePhone, Gender? gender = null, DateTime? birthDate = null, int? suffixValueId = null)
        {
            FirstName = firstName.IsNullOrEmpty() ? firstName.Trim() : string.Empty;
            LastName = lastName.IsNullOrEmpty() ? lastName.Trim() : string.Empty;
            Email = email.IsNullOrEmpty() ? email.Trim() : string.Empty;
            MobilePhone = mobilePhone.IsNullOrEmpty() ? PhoneNumber.CleanNumber(mobilePhone) : string.Empty;
            Gender = gender;
            BirthDate = birthDate;
            SuffixValueId = suffixValueId;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone.
        /// </summary>
        /// <value>
        /// The mobile phone.
        /// </value>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the suffix value identifier.
        /// </summary>
        /// <value>
        /// The suffix value identifier.
        /// </value>
        public int? SuffixValueId { get; set; }
    }
}
