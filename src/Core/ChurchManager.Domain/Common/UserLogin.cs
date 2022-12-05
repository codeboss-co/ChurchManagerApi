using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChurchManager.Domain.Common
{
    public class UserLogin : Entity<int>, IAggregateRoot<int>
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }
        [MaxLength(128)]
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public List<string> Roles { get; set; } = new(0);

        public int PersonId { get; set; }

        [Required]
        [MaxLength(50)]
        [DefaultValue("Tenant1")]
        public string Tenant { get; set; }

        #region Navigation Properties

        public virtual Person Person { get; set; }

        #endregion
    }
}
