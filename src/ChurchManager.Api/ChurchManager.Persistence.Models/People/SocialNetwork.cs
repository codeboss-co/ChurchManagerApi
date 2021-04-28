using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Persistence.Models.People
{
    [Table("SocialNetwork")]

    public class SocialNetwork : Entity<int>
    {
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [MaxLength(50)]
        public string Value { get; set; }
    }
}
