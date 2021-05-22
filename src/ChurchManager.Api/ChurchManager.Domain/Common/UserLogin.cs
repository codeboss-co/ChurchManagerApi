using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.People;

namespace ChurchManager.Domain.Common
{
    public class UserLogin
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }
        [MaxLength(128)]
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public List<string> Roles { get; set; } = new List<string>(0);

        public int PersonId { get; set; }

        #region Navigation Properties

        public virtual Person Person { get; set; }

        #endregion
    }
}
