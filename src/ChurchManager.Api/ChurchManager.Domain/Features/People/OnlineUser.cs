using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.People
{
    [Table("OnlineUser")]

    public class OnlineUser : IAggregateRoot<int>
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }

        [MaxLength(50)]
        public string ConnectionId { get; set; }
        [MaxLength(20)] 
        public string Status { get; set; } = "online"; // offline
        public DateTimeOffset LastOnlineDateTime { get; set; } = DateTimeOffset.UtcNow;

        #region Navigation

        public virtual Person Person { get; set; }

        #endregion

        public void GoOnline()
        {
            Status = "online";
            LastOnlineDateTime = DateTime.UtcNow;
        }

        public void GoOffline()
        {
            Status = "offline";
            LastOnlineDateTime = DateTime.UtcNow;
        }

        public bool IsOnline => Status == "online";
    }
}
