using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Shared.Persistence;

namespace People.Persistence.Models
{
    [Table("SocialNetwork", Schema = "People")]

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
