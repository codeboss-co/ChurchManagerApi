using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace Shared.Kernel
{
    [Table("PhoneNumber")]

    public class PhoneNumber : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number without country code. The number is stored without any string formatting. (i.e. (502) 555-1212 will be stored as 5025551212). This property is required.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the extension (if any) that would need to be dialed to contact the owner. 
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets an optional description of the PhoneNumber.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the number has been opted in for SMS
        /// </summary>
        public bool IsMessagingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the PhoneNumber is unlisted or not.
        /// </summary>
        public bool IsUnlisted { get; set; }

        [NotMapped]
        public string FullNumber
        {
            get
            {
                _fullNumber = CountryCode + Number;
                return _fullNumber;
            }
        }

        private string _fullNumber;
    }
}
